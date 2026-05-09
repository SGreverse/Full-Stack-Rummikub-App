<%@ Page Title="Login" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        :root {
            --primary-blue: #3182CE;
            --dark-bg: #1A202C;
            --card-bg: #2D3748;
            --text-light: #F7FAFC;
            --text-gray: #A0AEC0;
            --border-color: #4A5568;
            --gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: var(--dark-bg);
            color: var(--text-light);
            min-height: 100vh;
            overflow-x: hidden;
        }

        .login-container {
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            background: radial-gradient(circle at 50% 50%, #4A5568 0%, var(--dark-bg) 100%);
        }

        .login-form {
            background: var(--card-bg);
            padding: 40px;
            border-radius: 20px;
            box-shadow: 0 20px 40px rgba(0, 0, 0, 0.3);
            width: 400px;
            animation: bounceIn 0.8s ease-out;
        }

        @keyframes bounceIn {
            0% {
                transform: scale(0.3);
                opacity: 0;
            }

            50% {
                transform: scale(1.05);
            }

            70% {
                transform: scale(0.9);
            }

            100% {
                transform: scale(1);
                opacity: 1;
            }
        }

        .login-title {
            text-align: center;
            font-size: 32px;
            margin-bottom: 30px;
            background: var(--gradient);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
        }

        .form-group {
            margin-bottom: 20px;
        }

            .form-group label {
                display: block;
                margin-bottom: 8px;
                color: var(--text-gray);
                font-weight: 500;
            }

            .form-group input {
                width: 100%;
                padding: 12px 15px;
                background: var(--dark-bg);
                border: 2px solid var(--border-color);
                border-radius: 10px;
                color: var(--text-light);
                font-size: 16px;
                transition: all 0.3s ease;
            }

                .form-group input:focus {
                    outline: none;
                    border-color: var(--primary-blue);
                    box-shadow: 0 0 0 3px rgba(49, 130, 206, 0.1);
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
            background: linear-gradient(135deg, var(--primary-blue), #2C5282);
        }


        .btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="loginPage" class="login-container">
        <div class="login-form">
            <h2 class="login-title">🎯 Rummikub Login</h2>
            <div class="form-group">
                <label for="username">Username</label>
                <input id="Username" runat="server" cssclass="form-control" required="required" />
            </div>
            <div class="form-group">
                <label for="password">Password</label>
                <input id="Password" runat="server" cssclass="form-control" textmode="Password" required="required" />
            </div>
            <asp:button id="btnLogin" runat="server" text="Login to Game" cssclass="btn" style="width: 100%; margin-top: 20px;" onclick="LoginButton_Click" />

            <b id="ErrorMessage" visible="false" text-align="center" color="red" runat="server">One of The Inputs is Wrong</b>

        </div>
    </div>
</asp:Content>
