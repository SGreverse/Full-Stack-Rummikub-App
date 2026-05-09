<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdminAllPlayers.aspx.cs" Inherits="AdminAllPlayers" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta charset="UTF-8">
    <title>Player Management - Admin</title>
    <style>
        :root {
            --primary-red: #E53E3E;
            --primary-blue: #3182CE;
            --primary-green: #38A169;
            --dark-bg: #1A202C;
            --card-bg: #2D3748;
            --text-light: #F7FAFC;
            --text-gray: #A0AEC0;
            --border-color: #4A5568;
            --hover-bg: #4A5568;
            --gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: var(--dark-bg);
            color: var(--text-light);
            min-height: 100vh;
            overflow-x: hidden;
        }

        .admin-container {
            max-width: 1400px;
            margin: 0 auto;
            padding: 20px;
        }

        .page-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 30px;
            padding-bottom: 20px;
            border-bottom: 2px solid var(--border-color);
        }

        .page-title {
            font-size: 36px;
            background: var(--gradient);
            -webkit-text-fill-color: transparent;
            -webkit-background-clip: text;
        }

        .btn {
            padding: 10px 20px;
            background: var(--gradient);
            color: white;
            border: none;
            border-radius: 25px;
            cursor: pointer;
            font-size: 14px;
            font-weight: 600;
            transition: all 0.3s ease;
            text-decoration: none;
            display: inline-block;
            text-align: center;
        }

        .btn-green {
            background: linear-gradient(135deg, var(--primary-green), #2F855A);
        }

        .btn-blue {
            background: linear-gradient(135deg, var(--primary-blue), #2C5282);
        }

        .btn-red {
            background: linear-gradient(135deg, var(--primary-red), #C53030);
        }

        .btn-small {
            padding: 5px 10px;
            font-size: 12px;
            border-radius: 15px;
        }

        .btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3);
        }

        .table-container {
            background: var(--card-bg);
            border-radius: 15px;
            padding: 20px;
            box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
            overflow-x: auto;
        }

        .data-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

            .data-table th,
            .data-table td {
                padding: 15px;
                text-align: left;
                border-bottom: 1px solid var(--border-color);
            }

            .data-table th {
                background: var(--dark-bg);
                font-weight: 600;
                color: var(--primary-blue);
                position: sticky;
                top: 0;
            }

            .data-table tr:hover {
                background: var(--hover-bg);
                transition: background 0.3s ease;
            }

        .action-buttons {
            display: flex;
            gap: 10px;
        }

        .loading {
            display: none;
            text-align: center;
            padding: 40px;
        }

        .spinner {
            border: 4px solid var(--border-color);
            border-top: 4px solid var(--primary-blue);
            border-radius: 50%;
            width: 40px;
            height: 40px;
            animation: spin 1s linear infinite;
            margin: 0 auto 20px;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="admin-container">
         <div class="page-header">
             <h1 class="page-title">Player Management</h1>
             <div>
                 <button type="button" class="btn btn-green" onclick="refreshPlayers()">Refresh Players</button>
                 <asp:Button type="button" Text="Add Player" runat="server" class="btn btn-blue" onclick="HeadToPlayerTableEdit" />
             </div>
         </div>
         <div class="table-container">
             <div class="loading" id="playersLoading">
                 <div class="spinner"></div>
                 <p>Loading players...</p>
             </div>
             <asp:GridView ID="PlayersGridView" runat="server" AutoGenerateColumns="False" CssClass="data-table" OnRowCommand="PlayerAction_RowCommand">
                 <Columns>
                     <asp:BoundField DataField="Username" HeaderText="Username" />
                     <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                     <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                     <asp:BoundField DataField="Email" HeaderText="Email" />
                     <asp:BoundField DataField="GamesPlayed" HeaderText="Games Played" />
                     <asp:BoundField DataField="GamesWon" HeaderText="Wins" />
                     <asp:TemplateField HeaderText="Admin">
                         <ItemTemplate>
                             <%# (bool)Eval("IsAdmin") ? "Yes" : "No" %>
                         </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Actions">
                         <ItemTemplate>
                             <asp:Button ID="Editbtn" runat="server" CssClass="btn btn-blue btn-small" Text="Edit" CommandName="EditPlayer" CommandArgument='<%#((GridViewRow)Container).RowIndex %>' />
                             <asp:Button ID="Deletebtn" runat="server" CssClass="btn btn-red btn-small" Text="Delete" CommandName="DeletePlayer" CommandArgument='<%#((GridViewRow)Container).RowIndex %>'/>
                         </ItemTemplate>
                     </asp:TemplateField>
                 </Columns>
             </asp:GridView>
         </div>
     </div>
</asp:Content>

