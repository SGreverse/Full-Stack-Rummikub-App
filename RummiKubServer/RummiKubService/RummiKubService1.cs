using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Security.Policy;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RummikubBusinessLogic;
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
namespace RummiKubService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class RummiKubService1 : IRummiKubService 
    {



        #region Service Methods

        #region Game Invites Methods
        public PostRequestStatusCode InviteToGame(int InvitingPlayerId, int invitedPlayerId)//invite a player to a  one on one game
        {
            return RummiKubLogic.InviteToGame(InvitingPlayerId, invitedPlayerId);
        }

        public PostRequestStatusCode InviteResponse(int invitedPlayerId, int InvitingPlayerId, bool IsAccepted)// player accepting or declining the offer
        {
            return RummiKubLogic.InviteResponse(invitedPlayerId, InvitingPlayerId, IsAccepted);
        }
        public GetRequestStatusCode<PlayerList> GetAllGameInvites(int PlayerId)
        {
            return RummiKubLogic.GetAllGameInvites(PlayerId);
        }
        #endregion



        #region Queue Methods
        public GetRequestStatusCode<string> CreatePrivateGame(int playerId)//create a private game
        {
            return RummiKubLogic.CreatePrivateGame(playerId);
        }
        public PostRequestStatusCode StartPrivateGame(string Code, int PartyLeaderPlayerId)//start the private game, only if all players are still online and not in another game
        {

            return RummiKubLogic.StartPrivateGame(Code, PartyLeaderPlayerId);
        }

        public PostRequestStatusCode ConnectToGame(int playerId, string GameCode)//connect to a public game
        {
            return RummiKubLogic.ConnectToGame(playerId, GameCode);

        }
        public GetRequestStatusCode<VisualDisplayOfGame> WaitForGame(int PlayerId)// ask the server if im still in the queue
        {
            return RummiKubLogic.WaitForGame(PlayerId);
        }
        public PostRequestStatusCode ExitWaitingQueue(int playerId, string Code)
        {
            return RummiKubLogic.ExitWaitingQueue(playerId, Code);
        }

        public GetRequestStatusCode<int> GetGameQueuePlayerAmount(string code)//get the amount of players in the queue for display
        {
            return RummiKubLogic.GetGameQueuePlayerAmount(code);
        }
        #endregion



        #region Game Methods

        public GetRequestStatusCode<Board> GetCurrentBoard(int playerId)
        {
            return RummiKubLogic.GetCurrentBoard(playerId);
        }
        public PostRequestStatusCode ExecuteActionInGame(GameAction action, int PlayerId)// gets the tiles and the action type from the client and executes it
        {
            return RummiKubLogic.ExecuteActionInGame(action, PlayerId);
        }
        public GetRequestStatusCode<Tile> TakeTileFromBank(int playerId)
        {

            return RummiKubLogic.TakeTileFromBank(playerId);
        }

        public GetRequestStatusCode<List<Tile>> TakeInitialTiles(int PlayerId)
        {
            return RummiKubLogic.TakeInitialTiles(PlayerId);

        }

        public GetRequestStatusCode<VisualDisplayOfGame> PlayerStateInGame(int PlayerId)//ask for the state of the game and all the players
        {

            return RummiKubLogic.PlayerStateInGame(PlayerId);
        }

        public PostRequestStatusCode EndTurn(int PlayerId)//checks if a player did a change to the board,if he didnt, give him a tile, otherwise just end the turn
        {
            return RummiKubLogic.EndTurn(PlayerId);

        }

        public PostRequestStatusCode ResetTurn(int PlayerId)// go back to the state of the game as it was at the start of a player's turn
        {
            return RummiKubLogic.ResetTurn(PlayerId);
        }

        public PostRequestStatusCode PlayerFinished(int PlayerId)//player has 0 tiles remaining
        {
            return RummiKubLogic.PlayerFinished(PlayerId);
        }
        public PostRequestStatusCode PlayerSurrended(int playerId)
        {
            return RummiKubLogic.PlayerSurrended(playerId);

        }
        public GetRequestStatusCode<List<Tile>> GetCurrentPlayerHand(int PlayerId)
        {
            return RummiKubLogic.GetCurrentPlayerHand(PlayerId);
        }


        public GetRequestStatusCode<bool> HasBoardChanged(int playerId)
        {
           return RummiKubLogic.HasBoardChanged(playerId);
        }

        public PostRequestStatusCode DropTileFromHand(int PlayerId, Tile t)//admin command  used for debugging
        {
            return RummiKubLogic.DropTileFromHand(PlayerId, t);
        }

        #endregion



        #region Image Method
        public GetRequestStatusCode<byte[]> GetImage(string fileName)//gives the user an image from the server icons file
        {
            return RummiKubLogic.GetImage(fileName);
        }
        public PostRequestStatusCode SaveImage(byte[] imageArray, string fileName)//saves the image inside the server icons file
        {

            return RummiKubLogic.SaveImage(imageArray, fileName);
        }
        public  GetRequestStatusCode<int> GetImageCount(string filename)
        {
            return RummiKubLogic.GetImageCount(filename);
        }


        #endregion



        #region DataBase Methods
        public GetRequestStatusCode<PlayerList> GetAllUsers()
        {
            return RummiKubLogic.GetAllUsers();
        }
        public GetRequestStatusCode<CityList> GetAllCities()
        {
            return RummiKubLogic.GetAllCities();
        }
        public GetRequestStatusCode<GameWinnerOrderList> GetAllGamesByUser(int userId)
        {
            return RummiKubLogic.GetAllGamesByUser(userId);
        }
        public GetRequestStatusCode<PlayerList> GetLeaderBoard()
        {
            return RummiKubLogic.GetLeaderBoard();
        }
        public PostRequestStatusCode DeletePlayer(int UserId)
        {
            return RummiKubLogic.DeletePlayer(UserId);
        }
        public PostRequestStatusCode EditPlayer(Player player)
        {
            return RummiKubLogic.EditPlayer(player);
        }
        #endregion


        #region Encryption

        public GetRequestStatusCode<string> GetPublicKey()//Used for the RSA encryption, returns the public key to the client
        {
            return new GetRequestStatusCode<string>(0, RSASecurity.GetPublicKey());
        }
         
        #endregion

        #region Login Methods
        public GetRequestStatusCode<Player> Login(string username, string EncryptedPass)//decrypts the password and 
        {
            return RummiKubLogic.Login(username, EncryptedPass);
        }
        public GetRequestStatusCode<Player> Register(Player player)
        {
            return RummiKubLogic.Register(player);

        }
        public PostRequestStatusCode IsUsernameUnique(string username)//for the register process
        {
            return RummiKubLogic.IsUsernameUnique(username);
        }
        public PostRequestStatusCode IsEmailUnique(string email)//for the register process
        {
            return RummiKubLogic.IsEmailUnique(email);
        }
        public GetRequestStatusCode<Player> GetPlayer(int UserId)
        {
            return RummiKubLogic.GetPlayer(UserId);
        }
        public PostRequestStatusCode UpdateUser(Player player)
        {
            return RummiKubLogic.UpdateUser(player);
        }
        public PostRequestStatusCode Logout(int PlayerId)//removes the player from the online players list
        {
            return RummiKubLogic.Logout(PlayerId);
        }
        #endregion




        #region Friend Methods
        public GetRequestStatusCode<PlayerList> GetFriends(int UserId, string usernamefilter)
        {

            return RummiKubLogic.GetFriends(UserId, usernamefilter);

        }
        public PostRequestStatusCode FriendRequest(int RequesterId, int SentForId)//send a friend request
        {
            return RummiKubLogic.FriendRequest(RequesterId, SentForId);
        }

        public PostRequestStatusCode ApproveFriend(int UserId, int requesterId)
        {
            return RummiKubLogic.ApproveFriend(UserId, requesterId);
        }

        public GetRequestStatusCode<PlayerList> GetAllRequests(int UserId, string usernamefilter)
        {
            return RummiKubLogic.GetAllRequests(UserId, usernamefilter);
        }
        public GetRequestStatusCode<PlayerList> GetAllNonFriends(int UserId, string usernamefilter)
        {
            return RummiKubLogic.GetAllNonFriends(UserId, usernamefilter);
        }

        #endregion



        #region Chat Methods
        public async Task<PostRequestStatusCode> AddMessageToGameChat(string message, int playerid)
        {
            return await RummiKubLogic.AddMessageToGameChat(message, playerid);
        }
        public GetRequestStatusCode<List<Message>> RecieveRecentGameChatMessage(int playerid)//recieves all the messages the player hasnt recieved yet
        {
            return RummiKubLogic.RecieveRecentGameChatMessage(playerid);
        }

        public GetRequestStatusCode<List<MessageAction>> GetAllChatMessages(int PlayerId, int OtherPlayerId, bool FirstRequest)// gets all the private messages from the DB if FirstRequest is true, otherwise gets the messages that were sent since the last request
        {
            return RummiKubLogic.GetAllChatMessages(PlayerId, OtherPlayerId, FirstRequest);
        }

        public PostRequestStatusCode SendMessageToPrivateChat(Message message)
        {
            return RummiKubLogic.SendMessageToPrivateChat(message);
        }

        public PostRequestStatusCode EditMessageInPrivateChat(Message message)
        {
            return RummiKubLogic.EditMessageInPrivateChat(message);

        }
        public PostRequestStatusCode DeleteMessageFromPrivateChat(Message message)
        {
            return RummiKubLogic.DeleteMessageFromPrivateChat(message);
        }
        public PostRequestStatusCode EnterPrivateChat(int PlayerId, int OtherPlayerId)
        {
            return RummiKubLogic.EnterPrivateChat(PlayerId, OtherPlayerId);
        }
        public PostRequestStatusCode ExitPrivateChat(int PlayerId)
        {
            return RummiKubLogic.ExitPrivateChat(PlayerId);

        }
        #endregion


        #region Contact Admins Methods
        public PostRequestStatusCode SendProblemToAdmins(UserProblem userProblem)// sends a problem to the admins so that they can resolve it
        {
            return RummiKubLogic.SendProblemToAdmins(userProblem);
        }

        public PostRequestStatusCode ResolveProblem(UserProblem userProblem)
        {
            return RummiKubLogic.ResolveProblem(userProblem);
        }
        public GetRequestStatusCode<UserProblemList> GetAllUserProblems()
        {
            return RummiKubLogic.GetAllUserProblems();
        }
        #endregion

        public List<string> GetAllDefaultIcons()// gets all the icons that will be displayed when you registered
        {
            return new List<string>() { "ManIcon.png", "WomanIcon.png" };
        }


        #endregion



        #region Empty Functions 
        //pass all the objects to the player that he needs to know so that he can know them so there won't be serilization problems
        // each is added only when i encounter a problem with serialization with a specific object
        public void PassSeries(TileSeries ts, RisingSeries rs, ColorSeries cs)
        { }

        public void PassTile(Joker j, Tile t, RummiColor c) { }

        public void PassGamePieces(Board b, PlayerState ps, GameStates gs, VisualDisplayOfGame g, VisualDisplayOfPlayer p, GameWinnerOrderList ol, GameWinnerOrder gwo) { }

        public void PassActions(GameAction a, InsertingSeriesAction ia, AddingTileAction aa) { }

        public void PassEntities(BaseEntity be, User u, PlayerInGame p) { }

        public void PassChatClasses(MessageAction ma, MessageActionType mat, Message m) { }

        #endregion
    }

}
