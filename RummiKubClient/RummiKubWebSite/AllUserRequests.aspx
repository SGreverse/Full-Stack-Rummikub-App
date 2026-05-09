<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AllUserRequests.aspx.cs" Inherits="AllUserRequests" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Admin Problems</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        :root {
            --primary-red: #E53E3E;
            --primary-blue: #3182CE;
            --primary-yellow: #D69E2E;
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

        .page {
            min-height: 100vh;
            padding: 20px;
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
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
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

        .btn-small {
            padding: 5px 10px;
            font-size: 12px;
            border-radius: 15px;
            cursor: pointer;
        }

        .btn-resolve {
            background: linear-gradient(135deg, var(--primary-green), #2F855A);
            color: white;
            border: none;
            transition: all 0.3s ease;
        }

            .btn-resolve:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3);
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page">
        <div class="admin-container">
            <div class="page-header">
                <h1 class="page-title">Admin Problems</h1>
            </div>
            <div class="table-container">
                <asp:GridView ID="PlayersGridView" runat="server" AutoGenerateColumns="False" CssClass="data-table" OnRowCommand="PlayerAction_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Subject" HeaderText="Subject" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="Resolve" runat="server" CssClass="btn btn-blue btn-small" Text="Look Into" CommandName="LookIntoProblem" CommandArgument='<%#((GridViewRow)Container).RowIndex %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

