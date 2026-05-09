<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AllGames.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        /* General Page Styles */
        body {
            font-family: Arial, sans-serif; /* A clean, readable font */
            background-color: #f4f8fc; /* A soft, light background color */
            margin: 0; /* Remove default body margins */
            padding: 0;
        }

        .container {
            width: 90%; /* Responsive width */
            max-width: 1200px; /* Maximum width for larger screens */
            margin: 20px auto; /* Center the content */
            padding: 20px;
            background-color: #fff; /* White container background */
            border-radius: 8px; /* Rounded corners */
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); /* Subtle shadow */
        }

        h1 {
            color: #333; /* Dark heading color */
            text-align: center; /* Center the title */
            margin-bottom: 20px;
        }

        /* DataList Container Styles */
        .datalist-container {
            display: flex;
            justify-content: center;
            width: 100%;
        }

        .game-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin-bottom: 20px; /* Space between game records */
            border: 1px solid #ddd; /* Add a subtle border */
            padding: 10px;
            border-radius: 5px;
            background-color: #f9f9f9; /* Light background color */
            width: 100%;
            max-width: 600px; /* Consistent width for game containers */
        }

        .player-table {
            width: 100%;
            border-collapse: collapse;
        }

        .player-table td {
            padding: 8px;
            text-align: center;
            border: 1px solid #eee; /* Lighter borders for cells */
        }

        .player-header {
            font-weight: bold;
            background-color: #e9ecef; /* Header background color */
        }

        .winner {
            font-weight: bold;
            color: #28a745; /* Green color for the winner */
        }

        /* Optional: Add hover effect */
        .player-table tr:hover {
            background-color: #f0f0f0;
        }

        .game-id {
            font-size: 0.8em;
            color: #6c757d; /* Muted color for Game ID */
            margin-top: 5px;
        }

        /* Add some spacing and style to the DataList when it's empty */
        #GameDataList[data-has-items='false'] {
            padding: 20px;
            text-align: center;
            font-style: italic;
            color: #888;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <h1 ID="GameHistoryHeader">User Game History</h1>  
        <div class="datalist-container">
            <asp:DataList ID="GameDataList" runat="server">
                <ItemTemplate>
                    <div class="game-container">
                        <table class="player-table">
                            <tr>
                                <td class="player-header">#1</td>
                                <td class="player-header">#2</td>
                                <td class="player-header">#3</td>
                                <td class="player-header">#4</td>
                            </tr>
                            <tr>
                                <td class="winner">
                                    <span><%# Eval("First") %></span>
                                </td>
                                <td>
                                    <span><%# Eval("Second") %></span>
                                </td>
                                <td>
                                    <span><%# Eval("Third") %></span>
                                </td>
                                <td>
                                    <span><%# Eval("Forth") %></span>
                                </td>
                            </tr>
                        </table>
                        <div class="game-id">
                            Game ID: <%# Eval("GameId") %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
</asp:Content>
