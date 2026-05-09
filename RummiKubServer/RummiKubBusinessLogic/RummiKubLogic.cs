using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses;
using RummiKubClasses.ChatClasses;
using RummiKubClasses.ChatObjects;
using RummiKubClasses.GameClasses;
using RummiKubClasses.GamePieces;
using RummiKubClasses.ObjectsForDataBase;
using RummiKubClasses.PlayingEntities;
using RummiKubClasses.Security;
using RummiKubClasses.StatusCodes;
using RummiKubClasses.Tiles;
using ViewModel;

namespace RummikubBusinessLogic
{
    public static class RummiKubLogic
    {
        static Dictionary<string, List<Player>> WaitingForGame = new Dictionary<string, List<Player>>()
        {
            {"2", new List<Player>() },
            {"3", new List<Player>() },
            { "4", new List<Player>() }
        };


        static Dictionary<int, (Player player, Game Players_Game, DateTime LastHeartBeat)> OnlinePlayers = new Dictionary<int, (Player player, Game Players_Game, DateTime LastHeartBeat)>();

        static GameInviteList AllGameInvites = new GameInviteList();

        static Dictionary<int, Chat> AllActiveChats = new Dictionary<int, Chat>();


        #region Game Invites Methods
        public static PostRequestStatusCode InviteToGame(int InvitingPlayerId, int invitedPlayerId)//invite a player to a  one on one game
        {
            if(!OnlinePlayers.ContainsKey(invitedPlayerId))
                return new PostRequestStatusCode(13);

            if(IsPlayerInGame(OnlinePlayers[InvitingPlayerId].player))
                return new PostRequestStatusCode(17);

            GameInvite gameInvite = new GameInvite() { Requester = InvitingPlayerId, Approver = invitedPlayerId };
            if (AllGameInvites.Contains(gameInvite))//if the invite already exists
            {
                return new PostRequestStatusCode(12);
            }
            else
            {
                AllGameInvites.Add(gameInvite);//add the invite to the list
                return new PostRequestStatusCode(0); 
            }

        }

        public static PostRequestStatusCode InviteResponse(int invitedPlayerId, int InvitingPlayerId, bool IsAccepted)// player accepting or declining the offer

        {
            if (!OnlinePlayers.ContainsKey(InvitingPlayerId))
            {
                return new PostRequestStatusCode(13);
                
            }

            if (IsPlayerInGame(OnlinePlayers[InvitingPlayerId].player))
            {
                return new PostRequestStatusCode(17);
            }

            GameInvite gameInvite = new GameInvite() { Requester = InvitingPlayerId, Approver = invitedPlayerId };
            if (IsAccepted)
            {
                Player InvitingPlayer = OnlinePlayers[InvitingPlayerId].player;
                Player InvitedPlayer = OnlinePlayers[invitedPlayerId].player;
                List<Player> waiting_players = new List<Player> { InvitedPlayer, InvitingPlayer };
                EnterPlayersToGame(GameType.Private, waiting_players);
                AllGameInvites.Remove(gameInvite);
                return new PostRequestStatusCode(0);
            }
            else
            {
                AllGameInvites.Remove(gameInvite);
                return new PostRequestStatusCode(0);
            }
        }        
        public static GetRequestStatusCode<PlayerList> GetAllGameInvites(int PlayerId)
        {
            List<int> AllPlayerIds = AllGameInvites.Where(_ => _.Approver == PlayerId).Select(_ => _.Requester).ToList();
            PlayerList players = new PlayerList();
            foreach (int Id in AllPlayerIds)
            {
                if (OnlinePlayers.ContainsKey(Id))
                    players.Add(OnlinePlayers[Id].player);
            }
            return new GetRequestStatusCode<PlayerList>(0, players);
        }
        #endregion



        #region Queue Methods
        public static GetRequestStatusCode<string> CreatePrivateGame(int playerId)//create a private game
        {
            if (!OnlinePlayers.ContainsKey(playerId))
                return new GetRequestStatusCode<string>(13);

            string code = GenerateGameCode();
            WaitingForGame.Add(code, new List<Player>());
            WaitingForGame[code].Add(OnlinePlayers[playerId].player);
            return new GetRequestStatusCode<string>(0, code);
        }
        public static PostRequestStatusCode StartPrivateGame(string Code, int PartyLeaderPlayerId)//start the private game, only if all players are still online and not in game
        {

            if (!WaitingForGame.ContainsKey(Code))//if the code doesnt exist
            {
                return new PostRequestStatusCode(5);
            }
            if (WaitingForGame[Code][0].Id != PartyLeaderPlayerId)//party leader is the first player on the list, and only he can start the game
            {
                return new PostRequestStatusCode(19);
            }
            foreach (Player player in WaitingForGame[Code])//if one of the players is in game or not online,remove him fro, the queue
            {
                if (!OnlinePlayers.ContainsKey(player.Id))
                {
                    WaitingForGame[Code].Remove(player);
                    return new PostRequestStatusCode(22);
                }
                if (IsPlayerInGame(player))
                {
                    WaitingForGame[Code].Remove(player);
                    return new PostRequestStatusCode(22);
                }

            }
            if (WaitingForGame[Code].Count < 2)//if player count is too low
                return new PostRequestStatusCode(20);

            EnterPlayersToGame(GameType.Private, WaitingForGame[Code]);
            WaitingForGame.Remove(Code);

            return new PostRequestStatusCode();
        }

        public static PostRequestStatusCode ConnectToGame(int playerId, string GameCode)
        {
            if (!OnlinePlayers.ContainsKey(playerId))
                return new PostRequestStatusCode(13);

            if (!WaitingForGame.ContainsKey(GameCode))
            {
                return new PostRequestStatusCode(24);
            }
            if (WaitingForGame[GameCode].Count == 4)
            {
                return new PostRequestStatusCode(21);
            }
            WaitingForGame[GameCode].Add(OnlinePlayers[playerId].player);

            if (GameCode.Length == 1 && WaitingForGame[GameCode].Count == GameCode[0] - '0')
            {
                EnterPlayersToGame(GameType.Public, WaitingForGame[GameCode]);
                WaitingForGame[GameCode].Clear();
            }

            return new PostRequestStatusCode(0);

        }
        public static GetRequestStatusCode<VisualDisplayOfGame> WaitForGame(int PlayerId)
        {
            if (!OnlinePlayers.ContainsKey(PlayerId))
                return new GetRequestStatusCode<VisualDisplayOfGame>(13);
            bool Is_In_Game = IsPlayerInGame(OnlinePlayers[PlayerId].player);
            if (Is_In_Game)
            {
                var status = GetGameById(PlayerId);
                if (status.Code != 0)
                    return new GetRequestStatusCode<VisualDisplayOfGame>(status.Code);

                Game game = status.ReturnedObject;
                List<VisualDisplayOfPlayer> players_state = GetDisplayItemsForAllPlayers(game, true);


                return new GetRequestStatusCode<VisualDisplayOfGame>(0, new VisualDisplayOfGame { Id = game.Id, GameState = game.GameState, visualDisplayOfPlayers = players_state });
            }
            OnlinePlayers[PlayerId] = (OnlinePlayers[PlayerId].player, null, DateTime.Now);
            return new GetRequestStatusCode<VisualDisplayOfGame>(1);
        }
        public static PostRequestStatusCode ExitWaitingQueue(int playerId, string Code)
        {
            if (!OnlinePlayers.ContainsKey(playerId))
                return new PostRequestStatusCode(13);

            if (!WaitingForGame.ContainsKey(Code))
                return new PostRequestStatusCode(24);

            if (IsPlayerInGame(OnlinePlayers[playerId].player))
                return new PostRequestStatusCode(6);

            WaitingForGame[Code].Remove(OnlinePlayers[playerId].player);
            return new PostRequestStatusCode();
        }

        public static GetRequestStatusCode<int> GetGameQueuePlayerAmount(string code)
        {
            try
            {
                return new GetRequestStatusCode<int>(0, WaitingForGame[code].Count);
            }
            catch
            {
                return new GetRequestStatusCode<int>(5);
            }
        }
        #endregion



        #region Game Methods
        public static GetRequestStatusCode<Board> GetDefaultBoard()
        {
            return new GetRequestStatusCode<Board>(0, new Board());
        }

        public static GetRequestStatusCode<Board> GetCurrentBoard(int playerId)
        {
            GetRequestStatusCode<Game> status = GetGameById(playerId);
            if (status.Code != 0)
                return new GetRequestStatusCode<Board>(status.Code);

            Game game = status.ReturnedObject;
            PlayerInGame asking_player = game.GetPlayerById(playerId);
            asking_player.PlayerUpdatedBoard();

            return new GetRequestStatusCode<Board>(0, game.GameBoard);
        }
        public static PostRequestStatusCode ExecuteActionInGame(GameAction action, int PlayerId)
        {
            GetRequestStatusCode<Game> status = GetGameById(PlayerId);
            if (status.Code != 0)
                return new PostRequestStatusCode(status.Code);

            Game game = status.ReturnedObject;

            action.Player = game.GetPlayerById(PlayerId);

            return action.ExecuteAction(game);
        }
        public static GetRequestStatusCode<Tile> TakeTileFromBank(int playerId)
        {

            GetRequestStatusCode<Game> status = GetGameById(playerId);

            if (status.Code != 0)
                return new GetRequestStatusCode<Tile>(status.Code);

            Game game = status.ReturnedObject as Game;
            PlayerInGame player = game.GetPlayerById(playerId);

            Tile drawn_tile = game.GameBank.Draw();

            player.Draw(drawn_tile);
            player.UpdateCopy();

            return new GetRequestStatusCode<Tile>(0, drawn_tile);
        }

        public static GetRequestStatusCode<List<Tile>> TakeInitialTiles(int PlayerId)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < 14; i++)
            {
                GetRequestStatusCode<Tile> status = TakeTileFromBank(PlayerId);

                if (status.Code != 0)
                    return new GetRequestStatusCode<List<Tile>>(status.Code);

                tiles.Add(status.ReturnedObject);
            }

            return new GetRequestStatusCode<List<Tile>>(0, tiles);

        }

        public static GetRequestStatusCode<VisualDisplayOfGame> PlayerStateInGame(int PlayerId)//timer for asking for the board
        {

            var status = GetGameById(PlayerId);
            if (status.Code != 0)
            {
                OnlinePlayers[PlayerId] = (OnlinePlayers[PlayerId].player, null, DateTime.Now);
                return new GetRequestStatusCode<VisualDisplayOfGame>(status.Code);
            }


            Game game = status.ReturnedObject;
            VisualDisplayOfGame gamevisual = new VisualDisplayOfGame(game.GameState, GetDisplayItemsForAllPlayers(game));

            OnlinePlayers[PlayerId] = (OnlinePlayers[PlayerId].player, game, DateTime.Now);
            return new GetRequestStatusCode<VisualDisplayOfGame>(0, gamevisual);
        }

        public static PostRequestStatusCode EndTurn(int PlayerId)//checks if a player did a change to the board,if he didnt, give him a tile, otherwise just end the turn
        {
            var status = GetGameById(PlayerId);
            if (status.Code != 0)
                return new PostRequestStatusCode(status.Code);

            Game game = status.ReturnedObject;
            PlayerInGame player = game.GetPlayerById(PlayerId);

            Board GameBoard = game.GameBoard;
            Board OriginalBoard = game.GameBoardCopy;


            if (GameBoard.ValidateBoard())
            {
                if (GameBoard.TilesOnBoard == OriginalBoard.TilesOnBoard)
                {
                    TakeTileFromBank(PlayerId);

                }
                if (player.playerBoard.Amount == 0)
                {
                    PlayerFinished(PlayerId);
                }
                else
                {
                    game.NextOne();
                }
                game.UpdateOriginalBoard();
                player.UpdateCopy();

                AlertPlayersAboutBoardChange(game);

                return new PostRequestStatusCode(0);
            }
            return new PostRequestStatusCode(4);

        }

        public static PostRequestStatusCode ResetTurn(int PlayerId)
        {
            var status = GetGameById(PlayerId);
            if (status.Code != 0)
                return new PostRequestStatusCode(status.Code);

            Game game = status.ReturnedObject;
            game.ReturnTurnBack(PlayerId);

            AlertPlayersAboutBoardChange(game);
            return new PostRequestStatusCode(0);
        }

        public static PostRequestStatusCode PlayerFinished(int PlayerId)
        {
            var status = GetGameById(PlayerId);
            if (status.Code != 0)
                return new PostRequestStatusCode(status.Code);

            Game game = status.ReturnedObject;
            game.AddWinningPlayer(PlayerId);

            if (game.GameState == GameStates.Ended)
            { //if the game ended
                if (game.GameType == GameType.Public)
                    UpdatePlayersScore(game);

                SaveGameResults(game);
            }
            return new PostRequestStatusCode();
        }
        public static PostRequestStatusCode PlayerSurrended(int playerId)//static because it is also called form the server, which why i made also the other 2 functions static
        {
            var status = GetGameById(playerId);
            if (status.Code != 0)
                return new PostRequestStatusCode(status.Code);

            Game game = status.ReturnedObject;

            PlayerInGame player = game.GetPlayerById(playerId);

            RemovePlayerFromGame(game, player);

            return new PostRequestStatusCode();

        }
        public static GetRequestStatusCode<List<Tile>> GetCurrentPlayerHand(int PlayerId)
        {
            var status = GetGameById(PlayerId);
            if (status.Code != 0)
                return new GetRequestStatusCode<List<Tile>>(status.Code);

            Game game = status.ReturnedObject;
            PlayerInGame player = game.GetPlayerById(PlayerId);

            return new GetRequestStatusCode<List<Tile>>(0, player.playerBoard.TileList);
        }


        public static GetRequestStatusCode<bool> HasBoardChanged(int playerId)
        {
            GetRequestStatusCode<Game> status = GetGameById(playerId);

            if (status.Code != 0)
                return new GetRequestStatusCode<bool>(status.Code);

            Game game = status.ReturnedObject;

            PlayerInGame player = game.GetPlayerById(playerId);

            return new GetRequestStatusCode<bool>(0, !player.Has_Updated_Board);//return if he didnt update the board which means the board has changed
        }

        public static PostRequestStatusCode DropTileFromHand(int PlayerId, Tile t)//admin command
        {
            var status = GetGameById(PlayerId);
            if (status.Code != 0)
                return new PostRequestStatusCode(status.Code);

            Game game = status.ReturnedObject;
            PlayerInGame player = game.GetPlayerById(PlayerId);
            player.Take(t);

            return new PostRequestStatusCode();
        }

        #endregion



        #region Image Method
        public static GetRequestStatusCode<byte[]> GetImage(string fileName)
        {
            // הפרמטר שמתקבל כולל רק שם הקובץ של התמונה 
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\" + "/ViewModel/Icons/" + fileName);
            byte[] imgArray = File.ReadAllBytes(path);


            return new GetRequestStatusCode<byte[]>(0, imgArray);
        }
        public static PostRequestStatusCode SaveImage(byte[] imageArray, string fileName)
        {

            var stream = new MemoryStream(imageArray);
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

            string localFilePath = Path.Combine(Environment.CurrentDirectory, @"..\..\..\" + "/ViewModel/Icons/" + fileName);
            img.Save(localFilePath);


            return new PostRequestStatusCode(0);
        }
        public static GetRequestStatusCode<int> GetImageCount(string filename)
        {
            string[] Allfiles = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, @"..\..\..\" + "/ViewModel/Icons/"));
            int count = Allfiles.Count(file => Path.GetFileName(file).Contains(filename));
            return new GetRequestStatusCode<int>(0,count);
        }
        #endregion



        #region DataBase Methods
        public static GetRequestStatusCode<PlayerList> GetAllUsers()
        {
            LeaderBoardDB db = new LeaderBoardDB();
            PlayerList lst = db.SelectAll();
            return new GetRequestStatusCode<PlayerList>(0, lst);
        }
        public static GetRequestStatusCode<CityList> GetAllCities()
        {
            CityDB db = new CityDB();
            CityList lst = db.SelectAll();
            return new GetRequestStatusCode<CityList>(0, lst);
        }
        public static GetRequestStatusCode<GameWinnerOrderList> GetAllGamesByUser(int userId)
        {
            AllPlayedGamesDB allPlayedGamesDB = new AllPlayedGamesDB();
            GameList allgames = allPlayedGamesDB.SelectByUserId(userId);
            return new GetRequestStatusCode<GameWinnerOrderList>(0, allPlayedGamesDB.ConvertGameToGameOrderList(allgames));
        }
        public static GetRequestStatusCode<PlayerList> GetLeaderBoard()
        {
            LeaderBoardDB db = new LeaderBoardDB();
            PlayerList lst = db.SelectLeaderBoard();

            return new GetRequestStatusCode<PlayerList>(0, lst);
        }
        public static PostRequestStatusCode DeletePlayer(int UserId)
        {
            LeaderBoardDB leaderBoardDB = new LeaderBoardDB();
            Player player = new Player() { Id = UserId };
            leaderBoardDB.Delete(player);
            BaseDB.SaveChanges();
            return new PostRequestStatusCode(0);
        }
        public static PostRequestStatusCode EditPlayer(Player player)
        {
            LeaderBoardDB leaderBoardDB = new LeaderBoardDB();
            leaderBoardDB.Update(player);
            BaseDB.SaveChanges();
            return new PostRequestStatusCode(0);
        }
        #endregion


        #region Encryption

        public static GetRequestStatusCode<string> GetPublicKey()
        {
            return new GetRequestStatusCode<string>(0, RSASecurity.GetPublicKey());
        }

        #endregion

        #region Login Methods
        public static GetRequestStatusCode<Player> Login(string username, string EncryptedPass)
        {
            LeaderBoardDB leaderBoardDB = new LeaderBoardDB();
            string DecryptedPass = RSASecurity.DecryptText(EncryptedPass);
            Player player = leaderBoardDB.SelectByLoginUserData(username);
            if (player == null|| !BcryptSecurity.IsSameValue(DecryptedPass,player.Password))
            {
                return new GetRequestStatusCode<Player>(9);
            }
            if (OnlinePlayers.ContainsKey(player.Id))
                return new GetRequestStatusCode<Player>(14);

            OnlinePlayers.Add(player.Id, (player, null, DateTime.Now));
            return new GetRequestStatusCode<Player>(0, player);
        }
        public static GetRequestStatusCode<Player> Register(Player player)
        {
            player.Password = BcryptSecurity.GenerateHash(RSASecurity.DecryptText(player.Password));//decrypt the password using the private key and encrypt using Bcrypt for storing it in the database


            LeaderBoardDB leaderBoardDB = new LeaderBoardDB();
            leaderBoardDB.Insert(player);
            BaseDB.SaveChanges();
            if (player.Id == 0)
                return new GetRequestStatusCode<Player>(11);
            OnlinePlayers.Add(player.Id, (player, null, DateTime.Now));
            return new GetRequestStatusCode<Player>(0, player);

        }
        public static PostRequestStatusCode IsUsernameUnique(string username)
        {
            LeaderBoardDB leaderboardDB = new LeaderBoardDB();
            PlayerList player = leaderboardDB.SelectByRegistrationUserData(username);
            if (player.Count > 0)
                return new PostRequestStatusCode(10);
            return new PostRequestStatusCode();
        }
        public static PostRequestStatusCode IsEmailUnique(string email)
        {
            LeaderBoardDB leaderboardDB = new LeaderBoardDB();
            PlayerList player = leaderboardDB.SelectByEmailInfo(email);
            if (player.Count > 0)
                return new PostRequestStatusCode(25);
            return new PostRequestStatusCode();
        }
        public static GetRequestStatusCode<Player> GetPlayer(int UserId)
        {
            LeaderBoardDB leaderboardDB = new LeaderBoardDB();
            Player player = leaderboardDB.SelectById(UserId);
            if (OnlinePlayers.ContainsKey(UserId))//updates the Player Info For all the other Onlien Users
            {
                OnlinePlayers[UserId] = (player, null, DateTime.Now);
            }
            return new GetRequestStatusCode<Player>(0, player);
        }
        public static PostRequestStatusCode UpdateUser(Player player)
        {
            LeaderBoardDB leaderboardDB = new LeaderBoardDB();
            leaderboardDB.Update(player);
            BaseDB.SaveChanges();
            return new PostRequestStatusCode();
        }
        public static PostRequestStatusCode Logout(int PlayerId)
        {
            if (OnlinePlayers.ContainsKey(PlayerId))
            {
                OnlinePlayers.Remove(PlayerId);
            }
            return new PostRequestStatusCode();
        }
        #endregion




        #region Friend Methods
        public static GetRequestStatusCode<PlayerList> GetFriends(int UserId, string usernamefilter)
        {

            LeaderBoardDB leaderBoardDB = new LeaderBoardDB();
            PlayerList players = leaderBoardDB.SelectFriends(UserId, usernamefilter);
            return new GetRequestStatusCode<PlayerList>(0, players);

        }
        public static PostRequestStatusCode FriendRequest(int RequesterId, int SentForId)
        {
            FriendDB friendDB = new FriendDB();
            friendDB.Insert(new FriendConnection() { Requester = RequesterId, Approver = SentForId });
            BaseDB.SaveChanges();

            return new PostRequestStatusCode();
        }

        public static PostRequestStatusCode ApproveFriend(int UserId, int requesterId)
        {
            FriendDB friendDB = new FriendDB();
            friendDB.Update(new FriendConnection() { Approver = UserId, Requester = requesterId });
            BaseDB.SaveChanges();
            return new PostRequestStatusCode();
        }

        public static GetRequestStatusCode<PlayerList> GetAllRequests(int UserId, string usernamefilter)
        {
            LeaderBoardDB leaderBoardDB = new LeaderBoardDB();
            return new GetRequestStatusCode<PlayerList>(0, leaderBoardDB.SelectFriendRequests(UserId, usernamefilter));
        }
        public static GetRequestStatusCode<PlayerList> GetAllNonFriends(int UserId, string usernamefilter)
        {
            LeaderBoardDB leaderBoardDB = new LeaderBoardDB();
            return new GetRequestStatusCode<PlayerList>(0, leaderBoardDB.SelectNonFriends(UserId, usernamefilter));
        }

        #endregion



        #region Chat Methods
        public static async Task<PostRequestStatusCode> AddMessageToGameChat(string message, int playerid)
        {
            var status = GetGameById(playerid);
            if (status.Code != 0)
                return new PostRequestStatusCode(status.Code);

            Game game = status.ReturnedObject;
            string family_friendly_text = await ProfanityDetector.TranslateToFamilyFriendslyText(message);
            game.AddMessageToChat(family_friendly_text, playerid);

            return new PostRequestStatusCode();
        }
        public static GetRequestStatusCode<List<Message>> RecieveRecentGameChatMessage(int playerid)
        {
            var status = GetGameById(playerid);
            if (status.Code != 0)
                return new GetRequestStatusCode<List<Message>>(status.Code);

            Game game = status.ReturnedObject;
            List<Message> messages = new List<Message>();

            while (!game.GameChat.IsEmpty(playerid))
                messages.Add(game.ReciveResentMessage(playerid));
            if (messages.Count == 0)
                return new GetRequestStatusCode<List<Message>>(1);
            return new GetRequestStatusCode<List<Message>>(0, messages);
        }

        public static GetRequestStatusCode<List<MessageAction>> GetAllChatMessages(int PlayerId, int OtherPlayerId, bool FirstRequest)
        {
            List<MessageAction> actions = new List<MessageAction>();
            if (FirstRequest)// if he just entered the chat, get all the messages from the db.
            {

                AllActiveChats[PlayerId].ClearChat(PlayerId);//clear all the chat messages he sent to him

                ChatDB db = new ChatDB();
                List<Message> messages = db.SelectAllChatMessages(PlayerId, OtherPlayerId);
                foreach (Message message in messages)
                {
                    actions.Add(new MessageAction(message));
                }
            }
            else// if its not his first request, give him all the messages in the chat
            {
                Chat chat = AllActiveChats[PlayerId];
                while (!chat.IsEmpty(PlayerId))
                {
                    actions.Add(chat.RecieveMessage(PlayerId));
                }
            }

            if (actions.Count == 0)
                return new GetRequestStatusCode<List<MessageAction>>(1, actions);
            return new GetRequestStatusCode<List<MessageAction>>(0, actions);
        }

        public static PostRequestStatusCode SendMessageToPrivateChat(Message message)
        {
            _ = Task.Run(async () =>
            {
                string filteredMsg = await ProfanityDetector.TranslateToFamilyFriendslyText(message.Msg);
                message.Msg = filteredMsg;
                AllActiveChats[message.SenderId].AddMessage(new MessageAction(message));
                ChatDB chatDB = new ChatDB();
                chatDB.Insert(message);
                BaseDB.SaveChanges();
            });

            return new PostRequestStatusCode(0);
        }

        public static PostRequestStatusCode EditMessageInPrivateChat(Message message)
        {
            _ = Task.Run(async () =>
            {

                string filteredMsg = await ProfanityDetector.TranslateToFamilyFriendslyText(message.Msg);
                message.Msg = filteredMsg;
                AllActiveChats[message.SenderId].AddMessage(new MessageAction(message));
                ChatDB chatDB = new ChatDB();
                chatDB.Update(message);
                BaseDB.SaveChanges();
            });
            return new PostRequestStatusCode();

        }
        public static PostRequestStatusCode DeleteMessageFromPrivateChat(Message message)
        {
            ChatDB chatDB = new ChatDB();
            chatDB.Delete(message);
            BaseDB.SaveChanges();
            AllActiveChats[message.SenderId].AddMessage(new MessageAction(message, MessageActionType.Delete));
            return new PostRequestStatusCode();
        }
        public static PostRequestStatusCode EnterPrivateChat(int PlayerId, int OtherPlayerId)
        {
            if (AllActiveChats.ContainsKey(PlayerId))
            {
                return new PostRequestStatusCode();

            }
            if (AllActiveChats.ContainsKey(OtherPlayerId) && AllActiveChats[OtherPlayerId].IsPlayerInChat(PlayerId))
            {
                AllActiveChats.Add(PlayerId, AllActiveChats[OtherPlayerId]);
            }
            else
            {
                AllActiveChats.Add(PlayerId, new Chat(PlayerId, OtherPlayerId));
            }
            return new PostRequestStatusCode();
        }
        public static PostRequestStatusCode ExitPrivateChat(int PlayerId)
        {
            if (AllActiveChats.ContainsKey(PlayerId))
            {
                AllActiveChats.Remove(PlayerId);
            }
            return new PostRequestStatusCode();

        }
        #endregion


        #region Contact Admins Methods
        public static PostRequestStatusCode SendProblemToAdmins(UserProblem userProblem)
        {
            LeaderBoardDB leaderBoardDB = new LeaderBoardDB();
            if (leaderBoardDB.SelectByEmailInfo(userProblem.Email).Count == 0)
            {
                return new PostRequestStatusCode(27);
            }
            ContactDB contactDB = new ContactDB();
            if (contactDB.SelectByEmail(userProblem.Email).Count > 5)
            {
                return new PostRequestStatusCode(28);
            }
            contactDB.Insert(userProblem);
            BaseDB.SaveChanges();
            return new PostRequestStatusCode(0);
        }

        public static PostRequestStatusCode ResolveProblem(UserProblem userProblem)
        {
            ContactDB contactDB = new ContactDB();
            contactDB.Delete(userProblem);
            BaseDB.SaveChanges();
            return new PostRequestStatusCode(0);
        }
        public static GetRequestStatusCode<UserProblemList> GetAllUserProblems()
        {
            ContactDB contactDB = new ContactDB();
            UserProblemList userProblems = contactDB.SelectAll();
            return new GetRequestStatusCode<UserProblemList>(0, userProblems);
        }

        #endregion

        public static List<string> GetAllDefaultIcons()
        {
            return new List<string>() { "ManIcon.png", "WomanIcon.png" };
        }






        #region Private methods
        private static List<VisualDisplayOfPlayer> GetDisplayItemsForAllPlayers(Game game, bool initialrequestflag = false)
        {
            List<VisualDisplayOfPlayer> players_state = new List<VisualDisplayOfPlayer>();

            foreach (PlayerInGame player in game.PlayersInGame.Values)
            {
                VisualDisplayOfPlayer playerdisplay = new VisualDisplayOfPlayer(player.Id, player.playerBoard.Amount, player.This_PlayerState);

                if (initialrequestflag)
                {
                    playerdisplay.IconName = player.IconName;
                    playerdisplay.Username = player.Username;
                    playerdisplay.Level = player.Level;
                }

                players_state.Add(playerdisplay);
            }
            return players_state;
        }

        private static void EnterPlayersToGame(GameType gameType, List<Player> waiting_players)
        {
            Dictionary<int, PlayerInGame> players_in_game = new Dictionary<int, PlayerInGame>();
            foreach (Player user in waiting_players)
            {
                players_in_game.Add(user.Id, new PlayerInGame(user));

            }

            Game game = new Game(players_in_game, gameType);
            AllPlayedGamesDB GamesDB = new AllPlayedGamesDB();
            VisualDisplayOfGame GameWithId = new VisualDisplayOfGame();
            GamesDB.Insert(GameWithId);//insert a new game to the database
            BaseDB.SaveChanges();
            game.Id = GameWithId.Id;// a way of going aorund the limitations of Game without an Id
            foreach (Player player in waiting_players)
            {
                OnlinePlayers[player.Id] = (player, game, DateTime.Now);
            }

        }
        private static void AlertPlayersAboutBoardChange(Game game)
        {

            foreach (PlayerInGame p in game.PlayersInGame.Values)
            {
                p.APlayerChangedTheBoard();
            }

        }
        private static string GenerateGameCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] code = new char[9];
            while (true)
            {
                for (int i = 0; i < 9; i++)
                {
                    code[i] = chars[random.Next(chars.Length)];
                }
                if (!WaitingForGame.ContainsKey(new string(code)))
                {
                    break;
                }
            }
            return new string(code);
        }
        private static GetRequestStatusCode<Game> GetGameById(int PlayerId)
        {
            try
            {
                Game game = OnlinePlayers[PlayerId].Players_Game;
                return new GetRequestStatusCode<Game>(0, game);
            }
            catch
            {
                return new GetRequestStatusCode<Game>(5);
            }

        }
        private static bool IsPlayerInGame(Player p)
        {
            bool Is_In_Game = OnlinePlayers.ContainsKey(p.Id) && OnlinePlayers[p.Id].Players_Game != null &&
               OnlinePlayers[p.Id].Players_Game.GameState != GameStates.Ended;
            return Is_In_Game;

        }


        private static void RemovePlayerFromGame(Game game, PlayerInGame player)//static because it is called form the server, which why i made also the other 2 functions static
        {

            game.PlayerSurrended(player);
            if (game.GameState == GameStates.Ended)
            { //if the game ended
                if (game.GameType == GameType.Public)
                {
                    UpdatePlayersScore(game);
                }
                SaveGameResults(game);

            }

        }
        #endregion

        #region HostMethods
        public static void RemoveNotLoadedPlayers()
        {
            List<int> players_to_remove = new List<int>();
            foreach ((Player player, Game Players_Game, DateTime LastHeartBeat) PlayerHeartBeat in OnlinePlayers.Values)
            {
                if ((DateTime.Now - PlayerHeartBeat.LastHeartBeat).TotalSeconds > 10)//checks if the time since last heartbeat is too big
                {
                    if (PlayerHeartBeat.Players_Game != null)
                    {
                        Game game = PlayerHeartBeat.Players_Game;
                        PlayerInGame player = game.GetPlayerById(PlayerHeartBeat.player.Id);
                        if (game.GameState != GameStates.Ended)
                        {
                            RemovePlayerFromGame(game, player);

                        }
                    }
                    players_to_remove.Add(PlayerHeartBeat.player.Id);
                }
            }
            foreach (int playerId in players_to_remove)
            {
                OnlinePlayers.Remove(playerId);
            }
        }
        public static void RemoveOfflinePlayersFromQueue()
        {
            List<string> LobbiesToRemove = new List<string>();
            foreach (string Code in WaitingForGame.Keys)
            {
                WaitingForGame[Code].RemoveAll(player => !OnlinePlayers.ContainsKey(player.Id));
                if (Code.Length > 1)
                {

                    if (WaitingForGame[Code].Count == 0)
                        LobbiesToRemove.Add(Code);
                }

            }
            foreach (string code in LobbiesToRemove)
            {
                WaitingForGame.Remove(code);
            }
        }
        public static void ClearEmptyChats()
        {
            List<int> EmptyChats = new List<int>();
            foreach (int Id in AllActiveChats.Keys)
            {
                if (!OnlinePlayers.ContainsKey(Id))
                {
                    EmptyChats.Add(Id);
                }
            }
            foreach (int Id in EmptyChats)
            {
                AllActiveChats.Remove(Id);
            }
        }
        private static void UpdatePlayersScore(Game game)
        {
            LeaderBoardDB leaderBoardDB = new LeaderBoardDB();
            game.Updateplayers();
            for (int i = 0; i < game.WinnerOrderList.Count; i++)
            {
                leaderBoardDB.Update(game.WinnerOrderList[i]);
            }
            BaseDB.SaveChanges();
        }
        private static void SaveGameResults(Game game)
        {
            AllPlayedGamesDB allPlayedGamesDB = new AllPlayedGamesDB();

            VisualDisplayOfGame displayOfGame = new VisualDisplayOfGame(){ Id = game.Id,visualDisplayOfPlayers=new List<VisualDisplayOfPlayer>()};
            foreach(PlayerInGame player in game.PlayersInGame.Values)
            {
                displayOfGame.visualDisplayOfPlayers.Add(new VisualDisplayOfPlayer()
                {
                    IconName = player.IconName,
                    Username = player.Username,
                    Level = player.Level,
                    Id = player.Id,
                });
            }
            allPlayedGamesDB.Update(displayOfGame);
            BaseDB.SaveChanges();
        }
        #endregion











    }
}
