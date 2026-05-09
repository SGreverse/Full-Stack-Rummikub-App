<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlayerTableEdit.aspx.cs" Inherits="PlayerTableEdit" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <title>Player Editor - Admin</title>
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
            --input-bg: #4A5568;
            --input-focus: #63B3ED;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: var(--dark-bg);
            color: var(--text-light);
            min-height: 100vh;
            overflow-x: hidden;
        }

        .admin-container {
            max-width: 800px;
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
            margin: 5px;
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

        .btn-gray {
            background: linear-gradient(135deg, #718096, #4A5568);
        }

        .btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3);
        }

        .form-container {
            background: var(--card-bg);
            border-radius: 15px;
            padding: 30px;
            box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
        }

        .form-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px;
            margin-bottom: 20px;
        }

        .form-group {
            display: flex;
            flex-direction: column;
        }

        .form-group.full-width {
            grid-column: 1 / -1;
        }

        .form-label {
            font-weight: 600;
            margin-bottom: 8px;
            color: var(--primary-blue);
        }

        .form-input {
            padding: 12px 15px;
            background: var(--input-bg);
            border: 2px solid transparent;
            border-radius: 8px;
            color: var(--text-light);
            font-size: 14px;
            transition: all 0.3s ease;
        }

        .form-input:focus {
            outline: none;
            border-color: var(--input-focus);
            background: #4A5568;
        }

        .form-select {
            padding: 12px 15px;
            background: var(--input-bg);
            border: 2px solid transparent;
            border-radius: 8px;
            color: var(--text-light);
            font-size: 14px;
            transition: all 0.3s ease;
        }

        .form-select:focus {
            outline: none;
            border-color: var(--input-focus);
            background: #4A5568;
        }

        .checkbox-container {
            display: flex;
            align-items: center;
            gap: 10px;
            padding: 10px 0;
        }

        .form-checkbox {
            width: 20px;
            height: 20px;
            accent-color: var(--primary-blue);
        }

        .action-buttons {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid var(--border-color);
        }

        .alert {
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 20px;
            font-weight: 500;
        }

        .alert-success {
            background: rgba(56, 161, 105, 0.2);
            border: 1px solid var(--primary-green);
            color: #68D391;
        }

        .alert-error {
            background: rgba(229, 62, 62, 0.2);
            border: 1px solid var(--primary-red);
            color: #FC8181;
        }

        .back-link {
            color: var(--text-gray);
            text-decoration: none;
            transition: color 0.3s ease;
        }

        .back-link:hover {
            color: var(--primary-blue);
        }

        @media (max-width: 768px) {
            .form-grid {
                grid-template-columns: 1fr;
            }
            
            .action-buttons {
                flex-direction: column;
                gap: 15px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="admin-container">
            <div class="page-header">
                <h1 class="page-title">
                    <asp:Literal ID="PageTitleLiteral" runat="server" Text="Add Player" />
                </h1>
                <a href="AdminAllPlayers.aspx" class="back-link">← Back to Players</a>
            </div>

            <div class="form-container">
                <asp:Panel ID="MessagePanel" runat="server" Visible="false">
                    <div class="alert" id="alertDiv" runat="server">
                        <asp:Literal ID="MessageLiteral" runat="server" />
                    </div>
                </asp:Panel>

                <div class="form-grid">
                    <div class="form-group">
                        <label class="form-label">Username *</label>
                        <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="form-input" MaxLength="50" required="true" />
                        <asp:RequiredFieldValidator ID="UsernameValidator" runat="server" 
                            ControlToValidate="UsernameTextBox" 
                            ErrorMessage="Username is required" 
                            ForeColor="#FC8181" Display="Dynamic" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Email *</label>
                        <asp:TextBox ID="EmailTextBox" runat="server" CssClass="form-input" TextMode="Email" MaxLength="100" required="true" />
                        <asp:RequiredFieldValidator ID="EmailValidator" runat="server" 
                            ControlToValidate="EmailTextBox" 
                            ErrorMessage="Email is required" 
                            ForeColor="#FC8181" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="EmailFormatValidator" runat="server"
                            ControlToValidate="EmailTextBox"
                            ErrorMessage="Please enter a valid email address"
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                            ForeColor="#FC8181" Display="Dynamic" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">First Name</label>
                        <asp:TextBox ID="FirstNameTextBox" runat="server" CssClass="form-input" MaxLength="50" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Last Name</label>
                        <asp:TextBox ID="LastNameTextBox" runat="server" CssClass="form-input" MaxLength="50" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">City</label>
                        <asp:DropDownList ID="CityDropDownList" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Select a city..." Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label class="form-label">Icon Name</label>
                        <asp:TextBox ID="IconNameTextBox" runat="server" CssClass="form-input" MaxLength="50" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Experience Points</label>
                        <asp:TextBox ID="ExperiencePointsTextBox" runat="server" CssClass="form-input" TextMode="Number" min="0" />
                        <asp:RangeValidator ID="ExperienceValidator" runat="server"
                            ControlToValidate="ExperiencePointsTextBox"
                            MinimumValue="0" MaximumValue="999999"
                            Type="Integer"
                            ErrorMessage="Experience points must be between 0 and 999999"
                            ForeColor="#FC8181" Display="Dynamic" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Score</label>
                        <asp:TextBox ID="ScoreTextBox" runat="server" CssClass="form-input" TextMode="Number" min="0" />
                        <asp:RangeValidator ID="ScoreValidator" runat="server"
                            ControlToValidate="ScoreTextBox"
                            MinimumValue="0" MaximumValue="999999"
                            Type="Integer"
                            ErrorMessage="Score must be between 0 and 999999"
                            ForeColor="#FC8181" Display="Dynamic" />
                    </div>

                    <div class="form-group full-width">
                        <div class="checkbox-container">
                            <asp:CheckBox ID="IsAdminCheckBox" runat="server" CssClass="form-checkbox" />
                            <label class="form-label" style="margin-bottom: 0;">Admin User</label>
                        </div>
                    </div>
                </div>

                <div class="action-buttons">
                    <div>
                        <asp:Button ID="SaveButton" runat="server" CssClass="btn btn-green" Text="Save Player" OnClick="SaveButton_Click" />
                        <asp:Button ID="CancelButton" runat="server" CssClass="btn btn-gray" Text="Cancel" OnClick="CancelButton_Click" CausesValidation="false" />
                    </div>
                    <asp:Button ID="DeleteButton" runat="server" CssClass="btn btn-red" Text="Delete Player" 
                        OnClick="DeleteButton_Click" Visible="false" CausesValidation="false"
                        OnClientClick="return confirm('Are you sure you want to delete this player? This action cannot be undone.');" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>