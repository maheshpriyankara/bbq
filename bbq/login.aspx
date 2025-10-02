<%@ Page Language="C#" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>HRIS Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta charset="utf-8">
    <meta name="keywords" content="Ripley and BBK, Ripley & Marshall, BBK Partnership">

    <style>
        .message {
            padding: 10px;
            margin: 10px 0;
            border-radius: 4px;
            text-align: center;
            font-size: 14px;
        }

        .success {
            background: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }

        .error {
            background: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }

        .loading {
            color: #fff;
            text-align: center;
            padding: 10px;
        }

        .btn-login, .btn-clear, .btn-otp {
            padding: 12px;
            margin: 5px 0;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
            transition: background 0.3s;
        }

        .btn-login {
            background: #007bff;
            color: white;
            width: 100%;
        }

            .btn-login:hover {
                background: #0056b3;
            }

        .btn-clear {
            background: #6c757d;
            color: white;
            width: auto;
        }

            .btn-clear:hover {
                background: #545b62;
            }

        .btn-otp {
            background: #28a745;
            color: white;
            width: auto;
        }

            .btn-otp:hover {
                background: #1e7e34;
            }

        .field-icon {
            float: right;
            margin-right: 10px;
            margin-top: -25px;
            position: relative;
            z-index: 2;
            cursor: pointer;
        }

        field-group label {
            display: flex;
            align-items: center;
            gap: 8px;
            cursor: pointer;
        }

        .field-group input[type="checkbox"] {
            margin: 0;
            transform: scale(1.2);
        }

        .api-status {
            font-size: 12px;
            color: #888;
            text-align: center;
            margin-top: 10px;
        }

        .inactivity-warning {
            background: #fff3cd;
            color: #856404;
            border: 1px solid #ffeaa7;
            padding: 10px;
            margin: 10px 0;
            border-radius: 4px;
            text-align: center;
            display: none;
        }
    </style>

    <link rel="stylesheet" href="css/style.css" type="text/css" media="all">
    <link href="//fonts.googleapis.com/css?family=Mukta+Mahee:200,300,400,500,600,700,800" rel="stylesheet">
    <link rel="stylesheet" href="css/font-awesome.css" type="text/css" media="all">
</head>

<body>
    <div class="content-w3ls">
        <div class="agileits-grid">
            <div class="content-top-agile" style="background: #000000">
                <h2 style="text-align: center">HRIS Login</h2>
                <h3 style="text-align: center; color: #808080">HRIS Login</h3>
            </div>
            <div class="content-bottom">
                <form id="loginForm" method="post">
                    <div id="messageContainer"></div>

                    <div id="inactivityWarning" class="inactivity-warning">
                        You were logged out due to inactivity. Please login again.
                    </div>

                    <div class="field_w3ls">
                        <div class="field-group">
                            <input name="userID" id="text_nic" type="text" value="" placeholder="User ID | NIC" required />
                        </div>
                        <div class="field-group">
                            <input id="text_otp" type="password" name="password" value="" placeholder="Type your password or OTP here..." required />
                            <span toggle="#text_otp" class="fa fa-fw fa-eye field-icon toggle-password"></span>
                        </div>
                    </div>


                    <div class="checkbox-wrapper">
                        <input type="checkbox" id="rememberMe" name="rememberMe" />
                        <label for="rememberMe">Remember me</label>
                    </div>
                    <div class="wthree-field">
                        <button type="button" id="btnLogin" class="btn-login">Sign In</button>
                    </div>
                    <div style="text-align: right; margin-top: 10px;">
                        <button type="button" id="btnClear" class="btn-clear">Clear</button>
                    </div>

                    <div class="api-status" id="apiStatus">
                        Ready
                    </div>
                </form>
            </div>
        </div>
    </div>

    <script src="js/jquery-2.2.3.min.js"></script>
    <script>
        // API Configuration
        const API_CONFIG = {
            baseUrl: 'https://localhost:44341/api',
            endpoints: {
                login: '/auth/login',
                requestOTP: '/auth/request-otp',
                validateToken: '/auth/validate-token',
                logout: '/auth/logout',
                getUserSettings: '/auth/user-settings'
            }
        };

        function checkInactivityLogout() {
            const urlParams = new URLSearchParams(window.location.search);
            if (urlParams.get('inactivity') === 'true') {
                $('#inactivityWarning').show();
                showMessage('You were logged out due to inactivity. Please login again.', 'error');
            }

            if (sessionStorage.getItem('inactivityLogout') === 'true') {
                $('#inactivityWarning').show();
                const timeout = sessionStorage.getItem('inactivityTimeout') || 5;
                showMessage(`You were logged out due to ${timeout} minutes of inactivity. Please login again.`, 'error');
                sessionStorage.removeItem('inactivityLogout');
                sessionStorage.removeItem('inactivityTimeout');
            }
        }

        $(document).ready(function () {
            updateApiStatus('Ready');
            checkInactivityLogout();

            // Check for existing token on page load
            checkExistingToken();

            // Show/hide password
            $(".toggle-password").click(function () {
                $(this).toggleClass("fa-eye fa-eye-slash");
                var input = $($(this).attr("toggle"));
                if (input.attr("type") == "password") {
                    input.attr("type", "text");
                } else {
                    input.attr("type", "password");
                }
            });

            $("#btnLogin").click(function () {
                login();
            });

            $("#btnRequestOTP").click(function () {
                requestOTP();
            });

            $("#btnClear").click(function () {
                clearLogin();
            });

            $("#text_otp").keypress(function (e) {
                if (e.which == 13) {
                    login();
                }
            });
        });

        function checkExistingToken() {
            var token = getCookie('RememberToken');
            if (token) {
                validateToken(token);
            }
        }

        function login() {
            var userID = $("#text_nic").val().trim();
            var password = $("#text_otp").val().trim();
            var rememberMe = $("#rememberMe").is(':checked');

            if (!userID || !password) {
                showMessage('Please enter both User ID and Password/OTP', 'error');
                return;
            }

            showLoading('Authenticating...');
            updateApiStatus('Authenticating...');

            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.login,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    userID: userID,
                    password: password
                }),
                success: function (response) {
                    updateApiStatus('Authentication successful');

                    if (response.success && response.token) {
                        // Store basic auth data immediately
                        sessionStorage.setItem('AuthToken', response.token);
                        sessionStorage.setItem('UserID', userID);
                        sessionStorage.setItem('LoginTime', new Date().getTime());

                        // Fetch user-specific settings including inactivity timeout
                        fetchUserSettings(response.token, userID, rememberMe, response.user?.inactivityTimeout);
                    } else {
                        var errorMsg = response.message || 'Login failed';
                        showMessage(errorMsg, 'error');
                    }
                },
                error: function (xhr, status, error) {
                    updateApiStatus('Authentication failed');
                    showMessage('Login error: ' + error, 'error');
                }
            });
        }

        function fetchUserSettings(token, userID, rememberMe, timeoutFromLogin) {
            showLoading('Loading user settings...');
            updateApiStatus('Loading settings...');

            // If we already got timeout from login response, use it
            if (timeoutFromLogin) {
                completeLogin(token, userID, rememberMe, timeoutFromLogin);
                return;
            }

            // Otherwise fetch from user-settings endpoint
            $.ajax({
                url: API_CONFIG.baseUrl + '/auth/user-settings?token=' + encodeURIComponent(token),
                type: 'GET',
                success: function (settingsResponse) {
                    if (settingsResponse.success) {
                        const inactivityTimeout = settingsResponse.inactivityTimeout || 5;
                        completeLogin(token, userID, rememberMe, inactivityTimeout);
                    } else {
                        useDefaultSettings(token, userID, rememberMe);
                    }
                },
                error: function (xhr, status, error) {
                    useDefaultSettings(token, userID, rememberMe);
                }
            });
        }

        function completeLogin(token, userID, rememberMe, inactivityTimeout) {
            sessionStorage.setItem('lastActivity', new Date().getTime());
            sessionStorage.setItem('inactivityTimeout', inactivityTimeout);
            sessionStorage.removeItem('inactivityLogout');

            if (rememberMe) {
                setCookie('RememberToken', token, 7);
                setCookie('RememberUser', userID, 7);
                setCookie('UserInactivityTimeout', inactivityTimeout, 7);
                console.log('Remember me enabled for 7 days with timeout: ' + inactivityTimeout + ' minutes');
            } else {
                deleteCookie('RememberToken');
                deleteCookie('RememberUser');
                deleteCookie('UserInactivityTimeout');
            }

            showMessage('Login successful! Redirecting...', 'success');
            updateApiStatus('Settings loaded');

            setTimeout(function () {
                window.location.href = 'home.aspx';
            }, 1000);
        }

        function useDefaultSettings(token, userID, rememberMe) {
            console.log('Using default inactivity timeout');
            const defaultTimeout = 5;
            completeLogin(token, userID, rememberMe, defaultTimeout);
        }

        function requestOTP() {
            var userID = $("#text_nic").val().trim();

            if (!userID) {
                showMessage('Please enter your User ID first', 'error');
                return;
            }

            showLoading('Sending OTP...');
            updateApiStatus('Requesting OTP...');

            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.requestOTP,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    userID: userID
                }),
                success: function (response) {
                    updateApiStatus('OTP request successful');

                    if (response.success) {
                        showMessage('OTP sent successfully', 'success');
                        $("#text_otp").focus();
                    } else {
                        var errorMsg = response.message || 'Failed to send OTP';
                        showMessage(errorMsg, 'error');
                    }
                },
                error: function (xhr, status, error) {
                    updateApiStatus('OTP request failed');
                    showMessage('Error sending OTP: ' + error, 'error');
                }
            });
        }

        function validateToken(token) {
            updateApiStatus('Validating token...');

            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.validateToken,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ token: token }),
                success: function (response) {
                    if (response && response.valid) {
                        // Auto-login successful, redirect to home
                        window.location.href = 'home.aspx';
                    } else {
                        clearLocalData();
                        updateApiStatus('Session expired');
                    }
                },
                error: function () {
                    clearLocalData();
                    updateApiStatus('Validation failed');
                }
            });
        }

        function clearLogin() {
            $("#text_nic").val('');
            $("#text_otp").val('');
            $("#rememberMe").prop('checked', false);
            $("#inactivityWarning").hide();

            var token = getCookie('RememberToken');
            if (token) {
                logout(token);
            } else {
                showMessage('Login cleared', 'success');
                updateApiStatus('Ready');
            }
        }

        function logout(token) {
            updateApiStatus('Logging out...');

            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.logout,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ token: token }),
                success: function () {
                    clearLocalData();
                    showMessage('Logged out successfully', 'success');
                },
                error: function () {
                    clearLocalData();
                    showMessage('Logged out successfully', 'success');
                }
            });
        }

        function clearLocalData() {
            // Clear cookies
            deleteCookie('AuthToken');
            deleteCookie('UserID');
            deleteCookie('UserName');
            deleteCookie('RememberToken');
            deleteCookie('RememberUser');
            deleteCookie('UserInactivityTimeout');

            // Clear session storage
            sessionStorage.removeItem('AuthToken');
            sessionStorage.removeItem('UserID');
            sessionStorage.removeItem('LoginTime');
            sessionStorage.removeItem('lastActivity');
            sessionStorage.removeItem('inactivityTimeout');
            sessionStorage.removeItem('inactivityLogout');
        }

        function updateApiStatus(status) {
            $('#apiStatus').text('Status: ' + status);
        }

        function showMessage(message, type) {
            $('#messageContainer').html('<div class="message ' + type + '">' + message + '</div>');
        }

        function showLoading(message) {
            $('#messageContainer').html('<div class="loading">' + message + '</div>');
        }

        function setCookie(name, value, days) {
            var expires = "";
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                expires = "; expires=" + date.toUTCString();
            }
            document.cookie = name + "=" + encodeURIComponent(value) + expires + "; path=/";
        }

        function getCookie(name) {
            var nameEQ = name + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1, c.length);
                if (c.indexOf(nameEQ) == 0) return decodeURIComponent(c.substring(nameEQ.length, c.length));
            }
            return null;
        }

        function deleteCookie(name) {
            document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
        }
    </script>
</body>
</html>
