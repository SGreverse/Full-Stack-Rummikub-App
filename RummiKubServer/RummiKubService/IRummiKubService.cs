using RummiKubClasses;
using RummiKubClasses.ChatClasses;
using RummiKubClasses.GameClasses;
using RummiKubClasses.GamePieces;
using RummiKubClasses.ObjectsForDataBase;
using RummiKubClasses.PlayingEntities;
using RummiKubClasses.StatusCodes;
using RummiKubClasses.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
namespace RummiKubService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]


    public interface IRummiKubService
    {
        [OperationContract]

        PostRequestStatusCode InviteToGame(int InvitingPlayerId, int InvitedPlayerId);

        [OperationContract]

        PostRequestStatusCode InviteResponse(int invitedPlayerId, int InvitingPlayerId, bool IsAccepted);

        [OperationContract]

        GetRequestStatusCode<string> CreatePrivateGame(int PlayerId);

        [OperationContract]

        PostRequestStatusCode StartPrivateGame(string Code, int PartyLeaderPlayerId);

        [OperationContract]

        PostRequestStatusCode ConnectToGame(int Playerid ,string Code);

        [OperationContract]

        GetRequestStatusCode<VisualDisplayOfGame> WaitForGame(int PlayerId);

        [OperationContract]

        PostRequestStatusCode ExitWaitingQueue(int PlayerId, string Code);

        [OperationContract]

        GetRequestStatusCode<int> GetGameQueuePlayerAmount(string code );

        [OperationContract]

        PostRequestStatusCode ExecuteActionInGame(GameAction action, int PlayerId);

        [OperationContract]

        GetRequestStatusCode<Board> GetCurrentBoard( int PlayerId);


        [OperationContract]

        PostRequestStatusCode EndTurn(int PlayerId);

        [OperationContract]

        PostRequestStatusCode ResetTurn(int PlayerId);

        [OperationContract]

        PostRequestStatusCode PlayerFinished(int PlayerId);

        [OperationContract]
        PostRequestStatusCode PlayerSurrended(int playerId);

        [OperationContract]

        GetRequestStatusCode<List<Tile>> GetCurrentPlayerHand(int PlayerId); 
        
        [OperationContract]

        GetRequestStatusCode<Tile> TakeTileFromBank(int PlayerId);

        [OperationContract]

        GetRequestStatusCode<List<Tile>> TakeInitialTiles(int PlayerId);

        [OperationContract]

        GetRequestStatusCode<bool> HasBoardChanged(int PlayerId);

        [OperationContract]

        void PassTile(Joker j, Tile t,RummiColor c);

        [OperationContract]

        void PassSeries(TileSeries ts, RisingSeries rs, ColorSeries cs);

        [OperationContract]


        GetRequestStatusCode<Player> GetPlayer(int PlayerId);

        [OperationContract]

        GetRequestStatusCode<VisualDisplayOfGame> PlayerStateInGame(int PlayerId);


        [OperationContract]

        void PassGamePieces(Board b,PlayerState ps,GameStates gs, VisualDisplayOfGame g, VisualDisplayOfPlayer p,GameWinnerOrderList wol,GameWinnerOrder gwo);

        [OperationContract]

        void PassEntities(BaseEntity be, User u, PlayerInGame p);

        [OperationContract]

        void PassActions(GameAction a, InsertingSeriesAction ia, AddingTileAction aa);

        [OperationContract]

        void PassChatClasses(MessageAction ma, MessageActionType mat, Message m);


        [OperationContract]

        GetRequestStatusCode<PlayerList> GetAllUsers();

        [OperationContract]

        GetRequestStatusCode<CityList> GetAllCities();

        [OperationContract]

        GetRequestStatusCode<PlayerList> GetLeaderBoard();


        [OperationContract]


        GetRequestStatusCode<byte[]> GetImage(string filename);

        [OperationContract]

        PostRequestStatusCode SaveImage(byte[] imgArray, string fileName);

        [OperationContract]
        GetRequestStatusCode<int> GetImageCount(string filename);


        [OperationContract]

        GetRequestStatusCode<GameWinnerOrderList> GetAllGamesByUser(int UserId);

        [OperationContract]


        GetRequestStatusCode<Player> Login(string username, string password);

        [OperationContract]

        GetRequestStatusCode<string> GetPublicKey();

        [OperationContract]

        PostRequestStatusCode Logout(int UserId);

        [OperationContract]

        PostRequestStatusCode UpdateUser(Player player);

        [OperationContract]
        GetRequestStatusCode<PlayerList> GetAllGameInvites(int UserId);


        [OperationContract]
        GetRequestStatusCode<PlayerList> GetFriends(int UserId, string usernamefilter);

        [OperationContract]

        PostRequestStatusCode FriendRequest(int RequesterId, int SentForId);


        [OperationContract]

        PostRequestStatusCode ApproveFriend(int UserId,int Requester);

        [OperationContract]

        GetRequestStatusCode<PlayerList> GetAllRequests(int UserId, string usernamefilter);

        [OperationContract]

        GetRequestStatusCode<PlayerList> GetAllNonFriends(int UserId, string usernamefilter);



        [OperationContract]

        PostRequestStatusCode IsUsernameUnique(string username);

        [OperationContract]

        PostRequestStatusCode IsEmailUnique(string email);

        [OperationContract]

        GetRequestStatusCode<Player> Register(Player player);

        [OperationContract]


        PostRequestStatusCode DropTileFromHand(int PlayerId,Tile t);//only for testing win conditions


        [OperationContract]

        List<string> GetAllDefaultIcons();

        [OperationContract]

        Task<PostRequestStatusCode> AddMessageToGameChat(string message, int playerid);

        [OperationContract]
        GetRequestStatusCode<List<Message>> RecieveRecentGameChatMessage(int playerid);

        [OperationContract]

        PostRequestStatusCode SendMessageToPrivateChat(Message message);

        [OperationContract]

        PostRequestStatusCode EditMessageInPrivateChat(Message message);

        [OperationContract]

        PostRequestStatusCode DeleteMessageFromPrivateChat(Message message);

        [OperationContract]

        PostRequestStatusCode EnterPrivateChat(int PlayerId, int OtherPlayerId);

        [OperationContract]
        GetRequestStatusCode<List<MessageAction>> GetAllChatMessages(int PlayerId, int OtherPlayerId, bool FirstRequest);

        [OperationContract]
        PostRequestStatusCode ExitPrivateChat(int PlayerId);

        [OperationContract]

        PostRequestStatusCode SendProblemToAdmins(UserProblem userProblem);

        [OperationContract]

        PostRequestStatusCode ResolveProblem(UserProblem userProblem);

        [OperationContract]
        GetRequestStatusCode<UserProblemList> GetAllUserProblems();

        [OperationContract]
        PostRequestStatusCode DeletePlayer(int UserId);

        [OperationContract]

        PostRequestStatusCode EditPlayer(Player player);

    }



}
