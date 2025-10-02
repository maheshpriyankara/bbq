<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="profile_employee.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="bbq.profile_employee" EnableEventValidation="false" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <title>HRIS - Employee Profile</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="icon" href="favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" id="theme" href="css/theme-default.css" />
    <link href="jquery-ui.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.13.0/jquery-ui.min.js"></script>
    <script type="text/javascript" src="js/bootstrap-min.js"></script>
    <script type="text/javascript" src="js/api-config.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

    <style>
        /* Error Popup Styles */
        .error-popup {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: white;
            border-radius: 10px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.3);
            padding: 30px;
            text-align: center;
            z-index: 10000;
            min-width: 300px;
            border-left: 5px solid #dc3545;
            animation: popupFadeIn 0.3s ease-out;
        }

        .error-popup-content {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 15px;
        }

        .error-icon {
            width: 60px;
            height: 60px;
            background: #dc3545;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-size: 30px;
            font-weight: bold;
        }

        .error-popup h3 {
            color: #dc3545;
            margin: 0;
            font-size: 24px;
        }

        .error-popup p {
            color: #333;
            margin: 0;
            font-size: 16px;
        }

        .error-popup.fade-out {
            animation: popupFadeOut 0.3s ease-in forwards;
        }
        /* Success Popup Styles */
        .success-popup {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: white;
            border-radius: 10px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.3);
            padding: 30px;
            text-align: center;
            z-index: 10000;
            min-width: 300px;
            border-left: 5px solid #28a745;
            animation: popupFadeIn 0.3s ease-out;
        }

        .success-popup-content {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 15px;
        }

        .success-icon {
            width: 60px;
            height: 60px;
            background: #28a745;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-size: 30px;
            font-weight: bold;
        }

        .success-popup h3 {
            color: #28a745;
            margin: 0;
            font-size: 24px;
        }

        .success-popup p {
            color: #333;
            margin: 0;
            font-size: 16px;
        }

        .popup-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            z-index: 9999;
            animation: overlayFadeIn 0.3s ease-out;
        }

        @keyframes popupFadeIn {
            from {
                opacity: 0;
                transform: translate(-50%, -50%) scale(0.8);
            }

            to {
                opacity: 1;
                transform: translate(-50%, -50%) scale(1);
            }
        }

        @keyframes overlayFadeIn {
            from {
                opacity: 0;
            }

            to {
                opacity: 1;
            }
        }

        .success-popup.fade-out {
            animation: popupFadeOut 0.3s ease-in forwards;
        }

        @keyframes popupFadeOut {
            from {
                opacity: 1;
                transform: translate(-50%, -50%) scale(1);
            }

            to {
                opacity: 0;
                transform: translate(-50%, -50%) scale(0.8);
            }
        }

        .hidden {
            display: none;
        }

        #loading {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255, 255, 255, 0.8);
            z-index: 9999;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .spinner {
            border: 5px solid #f3f3f3;
            border-top: 5px solid #3498db;
            border-radius: 50%;
            width: 50px;
            height: 50px;
            animation: spin 2s linear infinite;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        .api-status {
            font-size: 12px;
            color: #888;
            text-align: center;
            margin-top: 10px;
        }

        .success-message {
            color: green;
            font-weight: bold;
            padding: 10px;
            background: #d4edda;
            border: 1px solid #c3e6cb;
        }

        .error-message {
            color: red;
            font-weight: bold;
            padding: 10px;
            background: #f8d7da;
            border: 1px solid #f5c6cb;
        }

        .btn-delete {
            background: #dc3545;
            color: white;
            border: none;
        }

            .btn-delete:hover {
                background: #c82333;
            }
    </style>

    <script>

        document.addEventListener("keydown", function (event) {
            if (event.key === "Enter") {
                event.preventDefault();
            }
        });
        let inactivityTimer;

        // ========== ADD THESE COOKIE UTILITY FUNCTIONS ==========
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
        // ========== END COOKIE UTILITY FUNCTIONS ==========

        $(window).on('load', function () {
            console.log('Window loaded completely');
            initializeApplication();

            $("#text_firstName_").on('input propertychange', function () {
                updateSystemName();
            });

            // Update system name when typing in last name
            $("#text_LastName").on('input propertychange', function () {
                updateSystemName();
            });

            // Optional: Also update on blur (when user leaves the field)
            $("#text_firstName_, #text_LastName").on('blur', function () {
                updateSystemName();
            });

            $("#text_nic").on('input propertychange', function () {
                nicExtract();
            });
        });

        function initializeApplication() {
            setupInactivityTimer();
            checkAuthentication();
            $('#btnLogout').on('click', performLogout);
        }

        function setupInactivityTimer() {
            // Reset timer on any user activity
            const events = ['mousemove', 'keypress', 'click', 'scroll', 'touchstart', 'mousedown'];

            events.forEach(event => {
                document.addEventListener(event, resetInactivityTimer, true);
            });

            resetInactivityTimer();
        }

        function resetInactivityTimer() {
            // Clear existing timer
            if (inactivityTimer) {
                clearTimeout(inactivityTimer);
            }

            // Get timeout from database (via sessionStorage or cookie)
            let inactivityTimeout = parseInt(sessionStorage.getItem('inactivityTimeout'));
            if (!inactivityTimeout) {
                inactivityTimeout = parseInt(getCookie('UserInactivityTimeout')) || 5;
                sessionStorage.setItem('inactivityTimeout', inactivityTimeout);
            }

            const timeoutMs = inactivityTimeout * 60 * 1000;

            // Set new timer with database-driven timeout
            inactivityTimer = setTimeout(logoutDueToInactivity, timeoutMs);

            // Update last activity time
            sessionStorage.setItem('lastActivity', new Date().getTime());

            console.log(`Inactivity timer reset: ${inactivityTimeout} minutes`);
        }

        function logoutDueToInactivity() {
            const inactivityTimeout = parseInt(sessionStorage.getItem('inactivityTimeout')) || 5;

            console.log(`Logging out due to ${inactivityTimeout} minutes of inactivity`);

            // Store timeout for login page message
            sessionStorage.setItem('inactivityLogout', 'true');
            sessionStorage.setItem('inactivityTimeout', inactivityTimeout);

            // Show logout message
            alert(`You have been logged out due to ${inactivityTimeout} minutes of inactivity.`);

            performAutoLogout();
        }

        function checkAuthentication() {
            // Check session storage first
            var token = sessionStorage.getItem('AuthToken');
            var lastActivity = sessionStorage.getItem('lastActivity');

            // If no session token, check remember me cookies
            if (!token) {
                token = getCookie('RememberToken');
                if (token) {
                    // Recreate session from remember me
                    var userID = getCookie('RememberUser');
                    var userTimeout = parseInt(getCookie('UserInactivityTimeout')) || 5;

                    sessionStorage.setItem('AuthToken', token);
                    sessionStorage.setItem('UserID', userID);
                    sessionStorage.setItem('LoginTime', new Date().getTime());
                    sessionStorage.setItem('inactivityTimeout', userTimeout);

                    // For remember me users, start fresh session (don't check last activity)
                    sessionStorage.setItem('lastActivity', new Date().getTime());

                    console.log('Auto-login from Remember Me with timeout: ' + userTimeout + ' minutes');
                    validateToken(token);
                    return;
                } else {
                    console.log('No active session found, redirecting to login');
                    window.location.href = 'login.aspx';
                    return;
                }
            }

            // For remember me users returning, always give fresh start
            if (getCookie('RememberToken')) {
                console.log('Remember me user - starting fresh session');
                sessionStorage.setItem('lastActivity', new Date().getTime());
                validateToken(token);
                return;
            }

            // Regular users: Check inactivity
            var inactivityTimeout = parseInt(sessionStorage.getItem('inactivityTimeout')) || 5;

            if (lastActivity) {
                const now = new Date().getTime();
                const timeSinceLastActivity = now - parseInt(lastActivity);
                const timeoutMs = inactivityTimeout * 60 * 1000;

                if (timeSinceLastActivity > timeoutMs) {
                    console.log(`Session expired due to ${inactivityTimeout} minutes of inactivity`);
                    sessionStorage.setItem('inactivityLogout', 'true');
                    sessionStorage.setItem('inactivityTimeout', inactivityTimeout);
                    clearAuthData();
                    window.location.href = 'login.aspx';
                    return;
                }
            }

            validateToken(token);
        }

        function validateToken(token) {
            console.log('Validating token...');

            $.ajax({
                url: 'https://localhost:44341/api/auth/validate-token',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ token: token }),
                success: function (response) {
                    if (response && response.valid) {
                        console.log('Token valid, loading dashboard...');
                        loadDashboard();
                    } else {
                        console.log('Token invalid, redirecting to login');
                        clearAuthData();
                        window.location.href = 'login.aspx';
                    }
                },
                error: function (xhr, status, error) {
                    console.log('Token validation error:', error);
                    clearAuthData();
                    window.location.href = 'login.aspx';
                }
            });
        }

        function loadDashboard() {
            console.log('Loading dashboard data...');

            // Get username from sessionStorage or cookie
            var userName = sessionStorage.getItem('UserID') || getCookie('RememberUser') || 'User';
            $('#div_username').text(userName);

            // Get and display user's inactivity timeout
            var inactivityTimeout = parseInt(sessionStorage.getItem('inactivityTimeout')) || 5;
            console.log(`User inactivity timeout: ${inactivityTimeout} minutes`);

            // Reset inactivity timer when dashboard loads
            resetInactivityTimer();
            initializeApplication();
            setupEventHandlers();

        }

        function performLogout() {
            // Clear the inactivity timer when user manually logs out
            if (inactivityTimer) {
                clearTimeout(inactivityTimer);
            }

            console.log('Logging out...');
            $('#loading').removeClass('hidden');

            var token = sessionStorage.getItem('AuthToken') || getCookie('RememberToken');

            if (token) {
                $.ajax({
                    url: 'https://localhost:44341/api/auth/logout',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ token: token }),
                    complete: function () {
                        clearAuthData();
                        window.location.href = 'login.aspx';
                    },
                    error: function () {
                        clearAuthData();
                        window.location.href = 'login.aspx';
                    }
                });
            } else {
                clearAuthData();
                window.location.href = 'login.aspx';
            }
        }

        function performAutoLogout() {
            var token = sessionStorage.getItem('AuthToken') || getCookie('RememberToken');

            if (token) {
                // Silent logout - don't show loading indicator
                $.ajax({
                    url: 'https://localhost:44341/api/auth/logout',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ token: token }),
                    complete: function () {
                        clearAuthData();
                        window.location.href = 'login.aspx';
                    },
                    error: function () {
                        clearAuthData();
                        window.location.href = 'login.aspx';
                    }
                });
            } else {
                clearAuthData();
                window.location.href = 'login.aspx';
            }
        }

        function clearAuthData() {
            // Clear session storage
            sessionStorage.removeItem('AuthToken');
            sessionStorage.removeItem('UserID');
            sessionStorage.removeItem('LoginTime');
            sessionStorage.removeItem('lastActivity');
            sessionStorage.removeItem('inactivityTimeout');
            sessionStorage.removeItem('inactivityLogout');

            // Clear cookies (including Remember me cookies on explicit logout)
            deleteCookie('AuthToken');
            deleteCookie('UserID');
            deleteCookie('UserName');
            deleteCookie('RememberToken');
            deleteCookie('RememberUser');
            deleteCookie('UserInactivityTimeout');

            // Clear inactivity timer
            if (inactivityTimer) {
                clearTimeout(inactivityTimer);
            }
        }

        function setupEventHandlers() {
            // Search functionality
            $("#text_employee").autocomplete({
                source: function (request, response) {
                    searchEmployees(request.term, response);
                },
                select: function (event, ui) {
                    loadEmployeeData(ui.item.value);
                }
            });

            // Button handlers
            $("#btn_loadEmployee").click(loadEmployeeByEPF);
            $("#btn_clear").click(clearForm);

            $(".btn-save").off('click.save').on('click.save', saveEmployeeData);
            $(".btn-delete").off('click.delete').on('click.delete', deleteEmployee);

            // Tab change handlers
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var target = $(e.target).attr("href");
                if (target === "#tab-five" || target === "#tab-six") {
                    loadAdditionalData(target);
                }
            });
        }

        function searchEmployees(searchTerm, callback) {
            var token = sessionStorage.getItem('AuthToken') || getCookie('RememberToken');

            showLoading();
            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.search + '?term=' + encodeURIComponent(searchTerm),
                type: 'GET',
                headers: { 'Authorization': 'Bearer ' + token },
                success: function (response) {
                    if (response.success) {
                        callback(response.data);
                    } else {
                        showMessage('Search failed: ' + response.message, 'error');
                    }
                },
                error: function (xhr, status, error) {
                    showMessage('Search error: ' + error, 'error');
                },
                complete: function () {
                    hideLoading();
                }
            });
        }

        function loadEmployeeByEPF() {
            var epfNo = $("#text_epfNoSearch").val().trim();
            var empNo = $("#text_empNo").val().trim();

            if (!epfNo && !empNo) {
                showMessage('Please enter EPF No or Employee No', 'error');
                return;
            }

            loadEmployeeData(epfNo || empNo);
        }

        function loadEmployeeData(identifier) {
            showLoading();
            var token = sessionStorage.getItem('AuthToken') || getCookie('RememberToken');

            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.employees + '/' + encodeURIComponent(identifier),
                type: 'GET',
                headers: { 'Authorization': 'Bearer ' + token },
                success: function (response) {
                    if (response.success) {
                        populateForm(response.data);
                        showMessage('Employee data loaded successfully', 'success');
                    } else {
                        showMessage('Employee not found: ' + response.message, 'error');
                    }
                },
                error: function (xhr, status, error) {
                    showMessage('Error loading employee: ' + error, 'error');
                },
                complete: function () {
                    hideLoading();
                }
            });
        }

        function populateForm(employeeData) {
            // Personal Data
            $("#list_title").val(employeeData.title);
            $("#text_initial").val(employeeData.initial);
            $("#text_firstName_").val(employeeData.firstName);
            $("#text_LastName").val(employeeData.lastName);
            $("#text_firstName").val(employeeData.systemName);
            $("#date_DOB").val(employeeData.dob);
            $("#list_gender").val(employeeData.gender);
            $("#text_nic").val(employeeData.nic);
            $("#list_maritalStatus").val(employeeData.maritalStatus);
            $("#list_bloodGroup").val(employeeData.bloodGroup);
            $("#text_drivingLicence").val(employeeData.drivingLicense);
            $("#list_religion").val(employeeData.religion);
            $("#list_nationality").val(employeeData.nationality);
            $("#list_race").val(employeeData.race);
            $("#text_mobileNo").val(employeeData.mobile);
            $("#text_landNo").val(employeeData.landPhone);
            $("#text_contactNo").val(employeeData.contactNo);
            $("#text_address").val(employeeData.residentialAddress);
            $("#text_address2").val(employeeData.permanentAddress);

            // Payroll Data
            $("#list_company").val(employeeData.companyId);
            $("#list_designation").val(employeeData.designationId);
            $("#list_department").val(employeeData.departmentId);
            $("#list_shiftBlock").val(employeeData.shiftBlockId);
            $("#date_DOA").val(employeeData.dateOfAppointment);
            $("#text_basic").val(employeeData.basicSalary);
            $("#text_budgetary").val(employeeData.budgetaryAllowance);
            $("#text_attendance").val(employeeData.attendanceAllowance);
            $("#text_attendanceID").val(employeeData.attendanceId);
            $("#check_epfPay").prop('checked', employeeData.epfPay);
            $("#text_epfNo").val(employeeData.epfNo);
            $("#check_resgined").prop('checked', employeeData.resigned);
            $("#date_resginedDate").val(employeeData.resignedDate);
            $("#check_blocked").prop('checked', employeeData.blockAttendance);

            // Allowances
            $("#text_fixedAllowances").val(employeeData.fixedAllowance);
            $("#text_mealAllowances").val(employeeData.mealAllowance);
            $("#text_specialAllowances").val(employeeData.specialAllowance);
            $("#text_theaterAllowances").val(employeeData.theaterAllowance);
            $("#text_icuAllowances").val(employeeData.icuAllowance);
            $("#text_transportAllowances").val(employeeData.transportAllowance);
            $("#text_accommodationAllowances").val(employeeData.accommodationAllowance);
            $("#text_fuelAllowances").val(employeeData.fuelAllowance);
            $("#text_allowances2").val(employeeData.allowance2);

            // Bank Details
            if (employeeData.paymentMethod === 'Bank') {
                $("#radio_bankPay").prop('checked', true);
            } else {
                $("#radio_cashPay").prop('checked', true);
            }
            $("#text_AccountNo").val(employeeData.accountNo);
            $("#text_bankCode").val(employeeData.bankCode);
            $("#text_branchCode").val(employeeData.branchCode);

            // Roaster Details
            $("#check_defaultRoaster").prop('checked', employeeData.defaultRoaster);
            $("#time_deafultRosterFirstShiftInTime").val(employeeData.firstShiftInTime);
            $("#list_defaultRoasterFirstShiftInDate").val(employeeData.firstShiftInDate);
            $("#time_deafultRosterFirstShiftOutTime").val(employeeData.firstShiftOutTime);
            $("#list_defaultRoasterFirstShiftOutDate").val(employeeData.firstShiftOutDate);
            $("#time_deafultRosterSecondShiftInTime").val(employeeData.secondShiftInTime);
            $("#list_deafultRosterSecondShiftInDate").val(employeeData.secondShiftInDate);
            $("#time_deafultRosterSecondShiftOutTime").val(employeeData.secondShiftOutTime);
            $("#list_deafultRosterSecondShiftOutDate").val(employeeData.secondShiftOutDate);

            // OT Settings
            if (employeeData.otCircle === 'DayBased') {
                $("#radio_otCircleDayBased").prop('checked', true);
            } else {
                $("#radio_otCircle180Hours").prop('checked', true);
            }
            $("#check_leaveExtra06Hours").prop('checked', employeeData.leaveExtra06Hours);
            $("#check_leaveExtra06Hourshalf").prop('checked', employeeData.leaveExtra06HoursHalf);
            $("#check_leaveExtra08Hours").prop('checked', employeeData.leaveExtra08Hours);
            $("#check_leaveExtra08HoursHalf").prop('checked', employeeData.leaveExtra08HoursHalf);

            // Emergency Contact
            $("#text_emContactPerson").val(employeeData.emergencyContactPerson);
            $("#text_emContactNumber").val(employeeData.emergencyContactNumber);
            $("#text_emAddress").val(employeeData.emergencyContactAddress);
            $("#text_emRelationship").val(employeeData.emergencyContactRelationship);
        }

        function saveEmployeeData() {
            // alert('hi');
            var saveButton = $(".btn-save");

            // Disable button immediately to prevent multiple clicks
            saveButton.prop('disabled', true);
            saveButton.text('Saving...');

            var employeeData = collectFormData();
            var token = sessionStorage.getItem('AuthToken') || getCookie('RememberToken');

            console.log('Sending employee data:', employeeData);
            showLoading();

            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.employees,
                type: 'POST',
                contentType: 'application/json',
                headers: { 'Authorization': 'Bearer ' + token },
                data: JSON.stringify(employeeData),
                success: function (response) {
                    if (response.success) {
                        showMessageSuccess('Employee data saved successfully', 'success');
                    } else {
                        showMessageError('Save failed: ' + response.message, 'error');
                    }
                },
                error: function (xhr, status, error) {
                    let errorMessage = 'Error saving data: ' + error;
                    if (xhr.responseJSON && xhr.responseJSON.errors) {
                        const errors = xhr.responseJSON.errors;
                        errorMessage += '\nValidation errors:';
                        for (const [key, value] of Object.entries(errors)) {
                            errorMessage += `\n- ${key}: ${value}`;
                        }
                    }
                    showMessage(errorMessage, 'error');
                },
                complete: function () {
                    hideLoading();
                    // Re-enable button after request completes
                    saveButton.prop('disabled', false);
                    saveButton.text('Save Changes');
                }
            });
        }
        function collectFormData() {
            // Helper function to handle empty date values
            const handleDateValue = (dateValue) => {
                return dateValue && dateValue.trim() !== '' ? dateValue : null;
            };

            // Helper function to handle empty numeric values
            const handleNumericValue = (numValue) => {
                const parsed = parseFloat(numValue);
                return !isNaN(parsed) ? parsed : 0;
            };

            // FIXED: Handle empty string values - return empty string instead of null
            const handleStringValue = (strValue) => {
                return strValue && strValue.trim() !== '' ? strValue.trim() : '';
            };

            // FIXED: Handle dropdown/select values with defaults
            const handleSelectValue = (selectValue, defaultValue = '') => {
                return selectValue && selectValue !== '' ? selectValue : defaultValue;
            };

            return {
                // Personal Data
                title: handleSelectValue($("#list_title").val()),
                initial: handleStringValue($("#text_initial").val()),
                firstName: handleStringValue($("#text_firstName_").val()) || 'Unknown',
                lastName: handleStringValue($("#text_LastName").val()) || 'Unknown',
                systemName: handleStringValue($("#text_firstName").val()) || 'Unknown',
                dob: handleDateValue($("#date_DOB").val()),
                gender: handleSelectValue($("#list_gender").val(), 'Other'),
                nic: handleStringValue($("#text_nic").val()),
                maritalStatus: handleSelectValue($("#list_maritalStatus").val(), 'Single'),
                bloodGroup: handleSelectValue($("#list_bloodGroup").val()),
                drivingLicense: handleStringValue($("#text_drivingLicence").val()),
                religion: handleSelectValue($("#list_religion").val()),
                nationality: handleSelectValue($("#list_nationality").val()),
                race: handleSelectValue($("#list_race").val()),
                mobile: handleStringValue($("#text_mobileNo").val()),
                landPhone: handleStringValue($("#text_landNo").val()),
                contactNo: handleStringValue($("#text_contactNo").val()),
                residentialAddress: handleStringValue($("#text_address").val()),
                permanentAddress: handleStringValue($("#text_address2").val()),

                // Payroll Data
                companyId: handleNumericValue($("#list_company").val()),
                designationId: handleNumericValue($("#list_designation").val()),
                departmentId: handleNumericValue($("#list_department").val()),
                shiftBlockId: handleNumericValue($("#list_shiftBlock").val()),
                dateOfAppointment: handleDateValue($("#date_DOA").val()),
                basicSalary: handleNumericValue($("#text_basic").val()),
                budgetaryAllowance: handleNumericValue($("#text_budgetary").val()),
                attendanceAllowance: handleNumericValue($("#text_attendance").val()),
                attendanceId: handleStringValue($("#text_attendanceID").val()),
                epfPay: $("#check_epfPay").is(':checked'),
                epfNo: handleStringValue($("#text_epfNo").val()) || '000',
                EmployeeNo: handleStringValue($("#text_epfNo").val()) || 'EPF000',// Default EPF number
                resigned: $("#check_resgined").is(':checked'),
                resignedDate: handleDateValue($("#date_resginedDate").val()),
                blockAttendance: $("#check_blocked").is(':checked'),

                // Allowances
                fixedAllowance: handleNumericValue($("#text_fixedAllowances").val()),
                mealAllowance: handleNumericValue($("#text_mealAllowances").val()),
                specialAllowance: handleNumericValue($("#text_specialAllowances").val()),
                theaterAllowance: handleNumericValue($("#text_theaterAllowances").val()),
                icuAllowance: handleNumericValue($("#text_icuAllowances").val()),
                transportAllowance: handleNumericValue($("#text_transportAllowances").val()),
                accommodationAllowance: handleNumericValue($("#text_accommodationAllowances").val()),
                fuelAllowance: handleNumericValue($("#text_fuelAllowances").val()),
                allowance2: handleNumericValue($("#text_allowances2").val()),

                // Bank Details - FIXED: Ensure no null values
                paymentMethod: $("#radio_bankPay").is(':checked') ? 'Bank' : 'Cash',
                accountNo: handleStringValue($("#text_AccountNo").val()),
                bankCode: handleStringValue($("#text_bankCode").val()),
                branchCode: handleStringValue($("#text_branchCode").val()),

                // Roaster Details
                defaultRoaster: $("#check_defaultRoaster").is(':checked'),
                firstShiftInTime: handleStringValue($("#time_deafultRosterFirstShiftInTime").val()),
                firstShiftInDate: handleStringValue($("#list_defaultRoasterFirstShiftInDate").val()),
                firstShiftOutTime: handleStringValue($("#time_deafultRosterFirstShiftOutTime").val()),
                firstShiftOutDate: handleStringValue($("#list_defaultRoasterFirstShiftOutDate").val()),
                secondShiftInTime: handleStringValue($("#time_deafultRosterSecondShiftInTime").val()),
                secondShiftInDate: handleStringValue($("#list_deafultRosterSecondShiftInDate").val()),
                secondShiftOutTime: handleStringValue($("#time_deafultRosterSecondShiftOutTime").val()),
                secondShiftOutDate: handleStringValue($("#list_deafultRosterSecondShiftOutDate").val()),

                // OT Settings
                otCircle: $("#radio_otCircleDayBased").is(':checked') ? 'DayBased' : '180Hours',
                leaveExtra06Hours: $("#check_leaveExtra06Hours").is(':checked'),
                leaveExtra06HoursHalf: $("#check_leaveExtra06Hourshalf").is(':checked'),
                leaveExtra08Hours: $("#check_leaveExtra08Hours").is(':checked'),
                leaveExtra08HoursHalf: $("#check_leaveExtra08HoursHalf").is(':checked'),

                // Emergency Contact
                emergencyContactPerson: handleStringValue($("#text_emContactPerson").val()),
                emergencyContactNumber: handleStringValue($("#text_emContactNumber").val()),
                emergencyContactAddress: handleStringValue($("#text_emAddress").val()),
                emergencyContactRelationship: handleStringValue($("#text_emRelationship").val())
            };
        }
        function deleteEmployee() {
            var epfNo = $("#text_epfNoSearch").val().trim();
            if (!epfNo) {
                showMessage('Please enter EPF No to delete', 'error');
                return;
            }

            if (!confirm('Are you sure you want to delete this employee?')) {
                return;
            }

            var token = sessionStorage.getItem('AuthToken') || getCookie('RememberToken');
            showLoading();

            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.employees + '/' + encodeURIComponent(epfNo),
                type: 'DELETE',
                headers: { 'Authorization': 'Bearer ' + token },
                success: function (response) {
                    if (response.success) {
                        showMessage('Employee deleted successfully', 'success');
                        clearForm();
                    } else {
                        showMessage('Delete failed: ' + response.message, 'error');
                    }
                },
                error: function (xhr, status, error) {
                    showMessage('Error deleting employee: ' + error, 'error');
                },
                complete: function () {
                    hideLoading();
                }
            });
        }

        function loadDropdownData() {
            var token = sessionStorage.getItem('AuthToken') || getCookie('RememberToken');

            // Load companies
            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.masters.companies,
                type: 'GET',
                headers: { 'Authorization': 'Bearer ' + token },
                success: function (response) {
                    if (response.success) {
                        populateDropdown('#list_company', response.data);
                    }
                }
            });

            // Load designations
            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.masters.designations,
                type: 'GET',
                headers: { 'Authorization': 'Bearer ' + token },
                success: function (response) {
                    if (response.success) {
                        populateDropdown('#list_designation', response.data);
                    }
                }
            });

            // Load departments
            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.masters.departments,
                type: 'GET',
                headers: { 'Authorization': 'Bearer ' + token },
                success: function (response) {
                    if (response.success) {
                        populateDropdown('#list_department', response.data);
                    }
                }
            });

            // Load shift blocks
            $.ajax({
                url: API_CONFIG.baseUrl + API_CONFIG.endpoints.masters.shiftblocks,
                type: 'GET',
                headers: { 'Authorization': 'Bearer ' + token },
                success: function (response) {
                    if (response.success) {
                        populateDropdown('#list_shiftBlock', response.data);
                    }
                }
            });
        }

        function populateDropdown(selector, data) {
            var dropdown = $(selector);
            dropdown.empty();
            dropdown.append($('<option></option>').val('').text('Select...'));
            $.each(data, function (index, item) {
                dropdown.append($('<option></option>').val(item.id).text(item.name));
            });
        }

        function clearForm() {
            $('input[type="text"]').val('');
            $('input[type="date"]').val('');
            $('input[type="time"]').val('');
            $('select').val('');
            $('input[type="checkbox"]').prop('checked', false);
            $('input[type="radio"]').prop('checked', false);
            $("#radio_cashPay").prop('checked', true);
            showMessage('Form cleared', 'success');
        }

        function showLoading() {
            $("#loading").removeClass("hidden");
        }

        function hideLoading() {
            $("#loading").addClass("hidden");
        }

        function showMessage(message, type) {
            var messageDiv = $('#messageContainer');
            messageDiv.removeClass('success-message error-message')
                .addClass(type + '-message')
                .text(message)
                .show();
            setTimeout(function () {
                messageDiv.fadeOut();
            }, 5000);
        }

        // Utility functions
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

        function clearAuthData() {
            sessionStorage.clear();
            document.cookie.split(";").forEach(function (c) {
                document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/");
            });
        }

        function showMessageSuccess(message, title = "Success!") {
            // Create overlay
            const overlay = document.createElement('div');
            overlay.className = 'popup-overlay';

            // Create popup
            const popup = document.createElement('div');
            popup.className = 'success-popup';

            popup.innerHTML = `
        <div class="success-popup-content">
            <div class="success-icon">✓</div>
            <h3>${title}</h3>
            <p>${message}</p>
            <button class="btn btn-success" onclick="closeSuccessPopup()">OK</button>
        </div>
    `;

            // Add to page
            document.body.appendChild(overlay);
            document.body.appendChild(popup);

            // Auto close after 3 seconds (optional)
            setTimeout(() => {
                if (document.body.contains(popup)) {
                    closeSuccessPopup();
                }
            }, 3000);

            // Close on overlay click
            overlay.addEventListener('click', closeSuccessPopup);
        }

        function closeSuccessPopup() {
            const popup = document.querySelector('.success-popup');
            const overlay = document.querySelector('.popup-overlay');

            if (popup) {
                popup.classList.add('fade-out');
                setTimeout(() => {
                    if (document.body.contains(popup)) {
                        document.body.removeChild(popup);
                    }
                }, 300);
            }

            if (overlay) {
                overlay.style.opacity = '0';
                setTimeout(() => {
                    if (document.body.contains(overlay)) {
                        document.body.removeChild(overlay);
                    }
                }, 300);
            }
        }
        function showMessageError(message, title = "Error!") {
            // Remove any existing popups first
            closeAllPopups();

            // Create overlay
            const overlay = document.createElement('div');
            overlay.className = 'popup-overlay';

            // Create popup
            const popup = document.createElement('div');
            popup.className = 'error-popup';

            popup.innerHTML = `
        <div class="error-popup-content">
            <div class="error-icon">✕</div>
            <h3>${title}</h3>
            <p>${message}</p>
            <button class="btn btn-danger" onclick="closeErrorPopup()">OK</button>
        </div>
    `;

            // Add to page
            document.body.appendChild(overlay);
            document.body.appendChild(popup);

            // Auto close after 5 seconds (longer for errors)
            setTimeout(() => {
                if (document.body.contains(popup)) {
                    closeErrorPopup();
                }
            }, 5000);

            // Close on overlay click
            overlay.addEventListener('click', closeErrorPopup);
        }

        function closeErrorPopup() {
            const popup = document.querySelector('.error-popup');
            const overlay = document.querySelector('.popup-overlay');

            if (popup) {
                popup.classList.add('fade-out');
                setTimeout(() => {
                    if (document.body.contains(popup)) {
                        document.body.removeChild(popup);
                    }
                }, 300);
            }

            if (overlay) {
                overlay.style.opacity = '0';
                setTimeout(() => {
                    if (document.body.contains(overlay)) {
                        document.body.removeChild(overlay);
                    }
                }, 300);
            }
        }

        // Utility function to close all popups
        function closeAllPopups() {
            const successPopup = document.querySelector('.success-popup');
            const errorPopup = document.querySelector('.error-popup');
            const overlay = document.querySelector('.popup-overlay');

            if (successPopup) document.body.removeChild(successPopup);
            if (errorPopup) document.body.removeChild(errorPopup);
            if (overlay) document.body.removeChild(overlay);
        }

        function updateSystemName() {
            const firstName = $("#text_firstName_").val().trim();
            const lastName = $("#text_LastName").val().trim();

            // Combine first and last name with a space
            const systemName = `${firstName} ${lastName}`.trim();

            // Update the system name field
            $("#text_firstName").val(systemName);
        }



        // NIC to Other Extract
        var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var totalDates = [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        function nicExtract() {
            
            const idNumber = $("#text_nic").val().trim();

            //Check the 3 digits for use month & day
            var checkThreeOld = idNumber.substring(2, 5);
            var checkThreeNew = idNumber.substring(4, 7);

            //Check the last digit of the OLD NIC
            var checkV = idNumber.endsWith('V');
            var checkv = idNumber.endsWith('v');

            //Check the first digit of the NEW NIC
            var checkOne = idNumber.startsWith('1');
            var checkTow = idNumber.startsWith('2');


            //Validation start here
            if (idNumber.length == 10 && (checkV == true || checkv == true) && (checkThreeOld <= 366 || (checkThreeOld >= 501 && checkThreeOld <= 866))) {
                oldNIC(idNumber);
            } else if (idNumber.length == 12 && (checkOne == true || checkTow == true) && (checkThreeNew <= 366 || (checkThreeNew >= 501 && checkThreeNew <= 866))) {
                newNIC(idNumber);
            } else {
                return;
            }

        }
        function oldNIC(idNumber) {
            if (!idNumber || idNumber.length < 10) return;

            try {
                const twoDigitYear = idNumber.substring(0, 2);
                const year = "19" + twoDigitYear;
                let dayOfYear = parseInt(idNumber.substring(2, 5));

                if (dayOfYear > 500) dayOfYear -= 500;
                const adjustedDayOfYear = dayOfYear - 1;
                const monthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

                let dayCount = adjustedDayOfYear;
                let month = 0;

                for (let i = 0; i < monthDays.length; i++) {
                    if (dayCount <= monthDays[i]) {
                        month = i;
                        break;
                    }
                    dayCount -= monthDays[i];
                }

                const formattedMonth = (month + 1).toString().padStart(2, '0');
                const formattedDay = dayCount.toString().padStart(2, '0');
                $("#date_DOB").val(`${year}-${formattedMonth}-${formattedDay}`);
                $("#list_gender").val(`${gender}`);

            } catch (error) {
            }
        }
        function newNIC(idNumber) {
            if (!idNumber || idNumber.length < 12) {
                console.error('Invalid NIC number');
                return;
            }

            try {
                var year = idNumber.substring(0, 4);
                var currentYear = new Date().getFullYear();
                if (parseInt(year) < 1900 || parseInt(year) > currentYear) {
                    return;
                }
                var dayOfYear = parseInt(idNumber.substring(4, 7));
                if (dayOfYear < 1 || dayOfYear > 866) {
                    return;
                }
                var gender = "Male";
                if (dayOfYear > 500) {
                    dayOfYear -= 500;
                    gender = "Female";
                }
                const adjustedDayOfYear = dayOfYear - 1;
                const months = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"];
                const monthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                var isLeapYear = (parseInt(year) % 4 === 0 && parseInt(year) % 100 !== 0) || (parseInt(year) % 400 === 0);
                if (isLeapYear) {
                    monthDays[1] = 29;
                }
                let dayCount = adjustedDayOfYear;
                let month = 0;

                for (let i = 0; i < monthDays.length; i++) {
                    if (dayCount <= monthDays[i]) {
                        month = i;
                        break;
                    }
                    dayCount -= monthDays[i];
                }

                const formattedMonth = months[month];
                const formattedDay = dayCount.toString().padStart(2, '0');
                $("#date_DOB").val(`${year}-${formattedMonth}-${formattedDay}`);
                $("#list_gender").val(`${gender}`);

            } catch (error) {
            }
        }
    </script>
</head>

<body runat="server">
    <form runat="server">
        <!-- Loading indicator -->
        <div id="loading" class="hidden">
            <div class="spinner"></div>
        </div>

        <!-- Message container -->
        <div id="messageContainer" style="display: none;"></div>

        <!-- START PAGE CONTAINER -->
        <div class="page-container">

            <!-- START PAGE SIDEBAR -->
            <div class="page-sidebar">
                <!-- START X-NAVIGATION -->
                <ul class="x-navigation">
                    <li class="xn-logo">
                        <a href="index.html">HRIS</a>
                        <a href="#" class="x-navigation-control"></a>
                    </li>
                    <li class="xn-profile">
                        <a href="#" class="profile-mini">
                            <img src="assets/images/users/avatar.jpg" alt="John Doe" />
                        </a>
                        <div class="profile">
                            <div class="profile-image">
                                <img src="assets/images/users/avatar.jpg" alt="John Doe" />
                            </div>
                            <div class="profile-data">
                                <div class="profile-data-name" id="div_username"></div>
                                <div class="profile-data-title">HR Admin</div>
                            </div>
                            <div class="profile-controls">
                                <a href="pages-profile.html" class="profile-control-left"><span class="fa fa-info"></span></a>
                                <a href="pages-messages.html" class="profile-control-right"><span class="fa fa-envelope"></span></a>
                            </div>
                        </div>
                    </li>
                    <li>
                        <a href="home.aspx"><span class="fa fa-desktop"></span><span class="xn-text">Dashboard</span></a>
                    </li>
                    <li class="xn-openable active">
                        <a href="#"><span class="fa fa-files-o"></span><span class="xn-text">Profiles</span></a>
                        <ul>
                            <li class="active"><a href="profile_employee.aspx"><span class="fa fa-image"></span>Employee Master</a></li>
                        </ul>
                    </li>
                    <li>
                        <a href="payroll.aspx"><span class="fa fa-file-text-o"></span><span class="xn-text">Payroll</span></a>
                    </li>
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-file-text-o"></span><span class="xn-text">Time & Attendance</span></a>
                        <ul>
                            <li><a href="attendance.aspx"><span class="fa fa-heart"></span>Attendance</a></li>
                            <li><a href="roaster.aspx"><span class="fa fa-cogs"></span>Roaster</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-cogs"></span><span class="xn-text">On-Going Function</span></a>
                        <ul>
                            <li><a href="deduction.aspx"><span class="fa fa-heart"></span>Deductions & Allowances</a></li>
                            <li><a href="leave.aspx"><span class="fa fa-square-o"></span>Leave</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-pencil"></span><span class="xn-text">Reports</span></a>
                        <ul>
                            <li><a href="report_salary_summary.aspx"><span class="fa fa-file-text-o"></span>Salary Summary (Employee)</a></li>
                            <li><a href="advance_reports.aspx"><span class="fa fa-file-text-o"></span>Other Reports</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-table"></span><span class="xn-text">Settings</span></a>
                        <ul>
                            <li><a href="settings_.aspx"><span class="fa fa-align-justify"></span>Settings</a></li>
                        </ul>
                    </li>
                </ul>
                <!-- END X-NAVIGATION -->
            </div>
            <!-- END PAGE SIDEBAR -->

            <!-- PAGE CONTENT -->
            <div class="page-content">

                <!-- START X-NAVIGATION VERTICAL -->
                <ul class="x-navigation x-navigation-horizontal x-navigation-panel">
                    <!-- TOGGLE NAVIGATION -->
                    <li class="xn-icon-button">
                        <a href="#" class="x-navigation-minimize"><span class="fa fa-dedent"></span></a>
                    </li>
                    <!-- END TOGGLE NAVIGATION -->

                    <!-- SIGN OUT -->
                    <li class="xn-icon-button pull-right">
                        <a href="#" class="mb-control" data-box="#mb-signout"><span class="fa fa-sign-out"></span></a>
                    </li>
                    <!-- END SIGN OUT -->
                    <!-- MESSAGES -->
                    <li class="xn-icon-button pull-right"></li>
                    <!-- END MESSAGES -->
                    <!-- TASKS -->
                    <li class="xn-icon-button pull-right"></li>
                    <!-- END TASKS -->
                </ul>
                <!-- END X-NAVIGATION VERTICAL -->

                <!-- START BREADCRUMB -->
                <ul class="breadcrumb">
                    <li><a href="#">Home</a></li>
                    <li class="active">Employee Profile</li>
                </ul>
                <!-- END BREADCRUMB -->

                <!-- PAGE CONTENT WRAPPER -->
                <div class="page-content-wrap">

                    <div class="row">
                        <div class="col-md-12">
                            <div class="block">
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">EPF No</label>
                                        <input type="text" class="form-control" id="text_epfNoSearch" placeholder="001" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group has-success">
                                        <label class="control-label">Employee No</label>
                                        <input type="text" class="form-control" id="text_empNo" placeholder="001" />
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="form-group has-success">
                                        <label class="control-label">Employee Name</label>
                                        <input type="text" class="form-control" id="text_employee" placeholder="Search Employee here...." />
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label" style="color: white">.</label>
                                        <button type="button" class="btn btn-warning form-control" id="btn_loadEmployee" style="font-weight: bold; color: white;">LOAD</button>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label" style="color: white">.</label>
                                        <button type="button" class="btn btn-success form-control" id="btn_clear" style="font-weight: bold;">CLEAR</button>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label" style="color: white">.</label>
                                        <button type="button" class="btn btn-danger form-control btn-delete" style="font-weight: bold;">DELETE</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- KEEP ALL YOUR EXISTING TAB CONTENT EXACTLY AS IT IS -->
                    <div class="row">
                        <div class="col-md-12">

                            <panel class="form-horizontal">

                                <div class="panel panel-default tabs">
                                    <ul class="nav nav-tabs" role="tablist">
                                        <li class="active"><a href="#tab-first" role="tab" data-toggle="tab">Personal Data</a></li>
                                        <li><a href="#tab-second" role="tab" data-toggle="tab">Payroll Data</a></li>
                                        <li><a href="#tab-third" role="tab" data-toggle="tab">Bank Detail's</a></li>
                                        <li><a href="#tab-four" role="tab" data-toggle="tab">Roaster Detail's</a></li>
                                        <li><a href="#tab-five" role="tab" data-toggle="tab">Qulification</a></li>
                                        <li><a href="#tab-six" role="tab" data-toggle="tab">Experience</a></li>
                                        <li><a href="#tab-seven" role="tab" data-toggle="tab">Emergency Contact</a></li>
                                    </ul>
                                    <div class="panel-body tab-content">
                                        <div class="tab-pane active" id="tab-first">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Title</label>
                                                        <div class="col-md-2">
                                                            <select class="form-control" id="list_title" runat="server">
                                                                <option></option>
                                                                <option>Mr.</option>
                                                                <option>Mrs.</option>
                                                                <option>Ms.</option>
                                                                <option>Miss.</option>
                                                                <option>Dr.</option>
                                                                <option>Madam.</option>
                                                                <option>Mx.</option>
                                                                <option>Prof.</option>
                                                            </select>
                                                        </div>

                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Initial</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_initial" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">First Name</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_firstName_" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Last Name</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_LastName" runat="server" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">System Name </label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_firstName" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label">DOB *</label>
                                                        <div class="col-md-3">

                                                            <asp:TextBox ID="date_DOB" CssClass="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Gender *</label>
                                                        <div class="col-md-2">
                                                            <select class="form-control" id="list_gender" runat="server">
                                                                <option></option>
                                                                <option>Male</option>
                                                                <option>FeMale</option>
                                                                <option>Other</option>

                                                            </select>
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">NIC *</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_nic" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">

                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Marital Status</label>
                                                        <div class="col-md-3">
                                                            <select class="form-control" id="list_maritalStatus" runat="server">
                                                                <option></option>
                                                                <option>Single</option>
                                                                <option>Married</option>
                                                                <option>Divocerd</option>
                                                                <option>Other</option>
                                                            </select>
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Blood Group</label>
                                                        <div class="col-md-3">
                                                            <select class="form-control" id="list_bloodGroup" runat="server">
                                                                <option></option>
                                                                <option>A+</option>
                                                                <option>A-</option>
                                                                <option>B+</option>
                                                                <option>B-</option>
                                                                <option>AB+</option>
                                                                <option>AB-</option>
                                                                <option>O+</option>
                                                                <option>O-</option>
                                                            </select>
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Driving Licence No</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_drivingLicence" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Religion</label>
                                                        <div class="col-md-3">
                                                            <select class="form-control" id="list_religion" runat="server">
                                                                <option></option>
                                                                <option>Buddhism</option>
                                                                <option>Burgher</option>
                                                                <option>Chirstian</option>
                                                                <option>Hindu</option>
                                                                <option>Islam</option>
                                                                <option>Roman Catholic</option>
                                                                <option>Other</option>
                                                            </select>
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Nationality</label>
                                                        <div class="col-md-3">
                                                            <select class="form-control" id="list_nationality" runat="server">
                                                                <option></option>
                                                                <option>Sri Lankan</option>
                                                                <option>Indian</option>
                                                                <option>Other</option>
                                                            </select>
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Race</label>
                                                        <div class="col-md-3">
                                                            <select class="form-control" id="list_race" runat="server">
                                                                <option></option>
                                                                <option>Sinhalese</option>
                                                                <option>Thamil</option>
                                                                <option>Burgher</option>
                                                                <option>Other</option>
                                                            </select>
                                                        </div>

                                                    </div>
                                                </div>
                                                <br />
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Mobile Number</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_mobileNo" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Land Number</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_landNo" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Payslip Send Number</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_contactNo" runat="server" title="" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Residental Address</label>
                                                        <div class="col-md-8 col-xs-12">
                                                            <asp:TextBox ID="text_address" runat="server" Class="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Permanet Address</label>
                                                        <div class="col-md-8 col-xs-12">
                                                            <asp:TextBox ID="text_address2" runat="server" Class="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="panel-footer">
                                                <button type="button" class="btn btn-primary pull-right btn-save">Save Changes</button>
                                            </div>

                                        </div>
                                        <div class="tab-pane" id="tab-second">
                                            <p style="color: #ff6a00">This information is used by the employer to calculate and process an employee's wages, taxes, and other deductions. It may also be used to generate reports and records for compliance with legal and regulatory requirements.</p>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">Company</label>
                                                        <div class="col-md-6 col-xs-12">

                                                            <asp:DropDownList CssClass="form-control" ID="list_company" runat="server" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">Designation </label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <asp:DropDownList CssClass="form-control" ID="list_designation" runat="server" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">Department </label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <asp:DropDownList CssClass="form-control" ID="list_department" runat="server" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">Shift Block</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <asp:DropDownList CssClass="form-control" ID="list_shiftBlock" runat="server" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">Date of Appointment</label>
                                                        <div class="col-md-4 col-xs-12">
                                                            <asp:TextBox ID="date_DOA" CssClass="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                </div>
                                                <div class="col-md-6">

                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Basic Salary</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_basic" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Budgetary Allowance</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_budgetary" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Attendance Allowance</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_attendance" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Attendance ID</label>
                                                        <div class="col-md-2">
                                                            <input type="text" class="form-control" value="" id="text_attendanceID" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label">EPF Pay</label>
                                                        <div class="col-md-9">
                                                            <label class="check">
                                                                <asp:CheckBox ID="check_epfPay" Text="" runat="server" />
                                                            </label>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">EPF No</label>
                                                        <div class="col-md-2">
                                                            <input type="text" class="form-control" value="" runat="server" id="text_epfNo" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label">Resigned</label>
                                                        <div class="col-md-9">
                                                            <label class="check">
                                                                <asp:CheckBox ID="check_resgined" Text="" runat="server" />
                                                            </label>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Resigned Date</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="date_resginedDate" CssClass="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label">No Attendance</label>
                                                        <div class="col-md-9">
                                                            <label class="check">
                                                                <asp:CheckBox ID="check_blocked" Text="" runat="server" />
                                                            </label>

                                                        </div>
                                                    </div>
                                                    <div class="panel-body">
                                                        <p style="color: coral">Fixed Allowances</p>
                                                        <div class="form-group">
                                                            <label class="col-md-3 col-xs-12 control-label">Fixed Allowance</label>
                                                            <div class="col-md-6 col-xs-12">
                                                                <input type="text" class="form-control" value="" id="text_fixedAllowances" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-3 col-xs-12 control-label">Meal Allowance</label>
                                                            <div class="col-md-6 col-xs-12">
                                                                <input type="text" class="form-control" value="" id="text_mealAllowances" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-3 col-xs-12 control-label">Speacil Allowance</label>
                                                            <div class="col-md-6 col-xs-12">
                                                                <input type="text" class="form-control" value="" id="text_specialAllowances" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-3 col-xs-12 control-label">Theater Allwoance</label>
                                                            <div class="col-md-6 col-xs-12">
                                                                <input type="text" class="form-control" value="" id="text_theaterAllowances" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-3 col-xs-12 control-label">ICU Allowance</label>
                                                            <div class="col-md-6 col-xs-12">
                                                                <input type="text" class="form-control" value="" id="text_icuAllowances" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-3 col-xs-12 control-label">Transport Allowance</label>
                                                            <div class="col-md-6 col-xs-12">
                                                                <input type="text" class="form-control" value="" id="text_transportAllowances" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-3 col-xs-12 control-label">Accommodation Alowance </label>
                                                            <div class="col-md-6 col-xs-12">
                                                                <input type="text" class="form-control" value="" id="text_accommodationAllowances" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-3 col-xs-12 control-label">Fuel Allowance</label>
                                                            <div class="col-md-6 col-xs-12">
                                                                <input type="text" class="form-control" value="" id="text_fuelAllowances" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-3 col-xs-12 control-label">Allowances 02</label>
                                                            <div class="col-md-6 col-xs-12">
                                                                <input type="text" class="form-control" value="" id="text_allowances2" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>


                                            <div class="panel-footer">
                                                <button type="button" class="btn btn-primary pull-right btn-save">Save Changes</button>
                                            </div>

                                        </div>
                                        <div class="tab-pane" id="tab-third">

                                            <div class="form-group" style="color: coral">
                                                <label class="col-md-3 col-xs-12 control-label">Cash Pay</label>
                                                <asp:RadioButton ID="radio_cashPay" Checked="true" class="form-control" runat="server" GroupName="bankpay" />
                                                <label class="col-md-3 col-xs-12 control-label">Bank Pay</label>
                                                <asp:RadioButton ID="radio_bankPay" class="form-control" runat="server" GroupName="bankpay" />
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Account No</label>
                                                <div class="col-md-2 col-xs-12">
                                                    <input type="text" class="form-control" id="text_AccountNo" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Bank Code</label>
                                                <div class="col-md-2 col-xs-12">
                                                    <input type="text" class="form-control" id="text_bankCode" runat="server" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Branch Code</label>
                                                <div class="col-md-2 col-xs-12">
                                                    <input type="text" class="form-control" id="text_branchCode" runat="server" />
                                                </div>
                                            </div>
                                            <div class="panel-footer">
                                                <button type="button" class="btn btn-primary pull-right btn-save">Save Changes</button>
                                            </div>


                                        </div>
                                        <div class="tab-pane" id="tab-four">
                                            <asp:CheckBox ID="check_defaultRoaster" Text="Default Roaster" runat="server" />
                                            <br />
                                            <br />
                                            <asp:Panel ID="panel_defaultRoaster" runat="server">
                                                <div class="col-md-6">

                                                    <p style="color: orange">First Shift</p>
                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">In Time</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <asp:TextBox ID="time_deafultRosterFirstShiftInTime" runat="server" TextMode="Time"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">In Date</label>
                                                        <div class="col-md-2 col-xs-12">
                                                            <select class="form-control" id="list_defaultRoasterFirstShiftInDate" runat="server">
                                                                <option></option>
                                                                <option>A</option>
                                                                <option>B</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">Out Time</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <asp:TextBox ID="time_deafultRosterFirstShiftOutTime" runat="server" TextMode="Time"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">Out Date</label>
                                                        <div class="col-md-2 col-xs-12">
                                                            <select class="form-control" id="list_defaultRoasterFirstShiftOutDate" runat="server">
                                                                <option></option>
                                                                <option>A</option>
                                                                <option>B</option>
                                                            </select>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="col-md-6">

                                                    <p style="color: orange">Second Shift</p>
                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">In Time</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <asp:TextBox ID="time_deafultRosterSecondShiftInTime" runat="server" TextMode="Time"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">In Date</label>
                                                        <div class="col-md-2 col-xs-12">
                                                            <select class="form-control" id="list_deafultRosterSecondShiftInDate" runat="server">
                                                                <option></option>
                                                                <option>A</option>
                                                                <option>B</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">Out Time</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <asp:TextBox ID="time_deafultRosterSecondShiftOutTime" runat="server" TextMode="Time"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-2 col-xs-12 control-label">Out Date</label>
                                                        <div class="col-md-2 col-xs-12">
                                                            <select class="form-control" id="list_deafultRosterSecondShiftOutDate" runat="server">
                                                                <option></option>
                                                                <option>A</option>
                                                                <option>B</option>
                                                            </select>
                                                        </div>
                                                    </div>

                                                </div>
                                            </asp:Panel>
                                            <br />
                                            <br />
                                            <div class="col-md-6">
                                                <br />
                                                <br />

                                                <div class="form-group">
                                                    <div class="col-md-2 col-xs-12">
                                                        <asp:RadioButton ID="radio_otCircleDayBased" runat="server" Text="OT CIRCLE TO DAY BASED" GroupName="otcircle"></asp:RadioButton>

                                                    </div>
                                                    <div class="col-md-2 col-xs-12">
                                                        <asp:RadioButton ID="radio_otCircle180Hours" runat="server" Text="OT CIRCLE 180 HOURS BASED" GroupName="otcircle"></asp:RadioButton>

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-10 col-xs-12">
                                                        <asp:CheckBox ID="check_leaveExtra06Hours" Text="LEAVE EXTRA 06 HOURS" runat="server" />

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-10 col-xs-12">
                                                        <asp:CheckBox ID="check_leaveExtra06Hourshalf" Text="LEAVE EXTRA 06 HOURS  HALF ( 03 HOURS )" runat="server" />

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-10 col-xs-12">
                                                        <asp:CheckBox ID="check_leaveExtra08Hours" Text="LEAVE EXTRA 08 HOURS" runat="server" />

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-10 col-xs-12">
                                                        <asp:CheckBox ID="check_leaveExtra08HoursHalf" Text="LEAVE EXTRA 08 HOURS  HALF ( 04 HOURS )" runat="server" />

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="panel-footer">
                                                <button type="button" class="btn btn-primary pull-right btn-save">Save Changes</button>
                                            </div>
                                        </div>
                                        <div class="tab-pane" id="tab-five">

                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Qulification Name</label>
                                                <div class="col-md-6 col-xs-12">
                                                    <input type="text" class="form-control" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Institute Name</label>
                                                <div class="col-md-6 col-xs-12">
                                                    <input type="text" class="form-control" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Year</label>
                                                <div class="col-md-2 col-xs-12">
                                                    <input type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Month</label>
                                                <div class="col-md-2 col-xs-12">
                                                    <input type="text" class="form-control" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Descrption</label>
                                                <div class="col-md-6 col-xs-12">
                                                    <textarea class="form-control" rows="5"></textarea>
                                                </div>
                                            </div>
                                            <div class="panel-footer">
                                                <button type="button" class="btn btn-primary pull-right btn-save">Save Changes</button>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">

                                                    <!-- START TIMELINE -->
                                                    <div class="timeline">

                                                        <!-- START TIMELINE ITEM -->
                                                        <div class="timeline-item timeline-main">
                                                            <div class="timeline-date">2021</div>
                                                        </div>
                                                        <!-- END TIMELINE ITEM -->

                                                        <!-- START TIMELINE ITEM -->
                                                        <div class="timeline-item">
                                                            <div class="timeline-item-info">2021 March</div>
                                                            <div class="timeline-item-icon"><span class="fa fa-globe"></span></div>
                                                            <div class="timeline-item-content">
                                                                <div class="timeline-heading">
                                                                    <img src="assets/images/users/nibm.png" />
                                                                    <a href="#">Diploma in Hardware & Networking </a>in <a href="#">NIBM</a>
                                                                </div>
                                                                <div class="timeline-body">

                                                                    <p>Identify Hardware Components</p>
                                                                    <p>Motherboard Components & Bus Architecture</p>
                                                                    <p>The CPU Evolution & Architecture</p>
                                                                    <p>Assembling and Disassembling of different types of PCs different types of Expansion cards</p>
                                                                    <p>Preparing the hard disk drive to install system software</p>
                                                                    <p>Installing and configuring Windows XP, Windows 2008, Windows 7 & Linux</p>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <!-- END TIMELINE ITEM -->

                                                        <!-- START TIMELINE ITEM -->
                                                        <div class="timeline-item timeline-item-right">
                                                            <div class="timeline-item-info">2019 Jan</div>
                                                            <div class="timeline-item-icon"><span class="fa fa-image"></span></div>
                                                            <div class="timeline-item-content">
                                                                <div class="timeline-heading">
                                                                    <img src="assets/images/users/ucsc.png" />
                                                                    <a href="#">Certificate in Office Package </a>in <a href="#">Univercity of Colombo</a>

                                                                </div>

                                                                <div class="timeline-body">

                                                                    <p>Identify Hardware Components</p>
                                                                    <p>Motherboard Components & Bus Architecture</p>
                                                                    <p>The CPU Evolution & Architecture</p>
                                                                    <p>Assembling and Disassembling of different types of PCs different types of Expansion cards</p>
                                                                    <p>Preparing the hard disk drive to install system software</p>
                                                                    <p>Installing and configuring Windows XP, Windows 2008, Windows 7 & Linux</p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- END TIMELINE ITEM -->

                                                        <!-- START TIMELINE ITEM -->
                                                        <div class="timeline-item">
                                                            <div class="timeline-item-info">2017 Decmber</div>
                                                            <div class="timeline-item-icon"><span class="fa fa-star"></span></div>
                                                            <div class="timeline-item-content">
                                                                <div class="timeline-heading" style="padding-bottom: 10px;">
                                                                    <img src="assets/images/users/wijaya.png" />
                                                                    <a href="#">Certificate in Graphic Desiging</a> in <a href="#">Wijaya Graphic Center</a>
                                                                </div>
                                                                <div class="timeline-body">

                                                                    <p>Identify Hardware Components</p>
                                                                    <p>Motherboard Components & Bus Architecture</p>
                                                                    <p>The CPU Evolution & Architecture</p>
                                                                    <p>Assembling and Disassembling of different types of PCs different types of Expansion cards</p>
                                                                    <p>Preparing the hard disk drive to install system software</p>
                                                                    <p>Installing and configuring Windows XP, Windows 2008, Windows 7 & Linux</p>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <!-- END TIMELINE ITEM -->
                                                        <!-- START TIMELINE ITEM -->
                                                        <div class="timeline-item timeline-item-right">
                                                            <div class="timeline-item-info">2015 Jan</div>
                                                            <div class="timeline-item-icon"><span class="fa fa-image"></span></div>
                                                            <div class="timeline-item-content">
                                                                <div class="timeline-heading">
                                                                    <img src="assets/images/users/ucsc.png" />
                                                                    <a href="#">Certificate in Office Package </a>in <a href="#">Univercity of Colombo</a>

                                                                </div>

                                                                <div class="timeline-body">

                                                                    <p>Identify Hardware Components</p>
                                                                    <p>Motherboard Components & Bus Architecture</p>
                                                                    <p>The CPU Evolution & Architecture</p>
                                                                    <p>Assembling and Disassembling of different types of PCs different types of Expansion cards</p>
                                                                    <p>Preparing the hard disk drive to install system software</p>
                                                                    <p>Installing and configuring Windows XP, Windows 2008, Windows 7 & Linux</p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- END TIMELINE ITEM -->









                                                    </div>
                                                    <!-- END TIMELINE -->

                                                </div>
                                            </div>
                                        </div>
                                        <div class="tab-pane" id="tab-six">

                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Position Name</label>
                                                <div class="col-md-6 col-xs-12">
                                                    <input type="text" class="form-control" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Company Name</label>
                                                <div class="col-md-6 col-xs-12">
                                                    <input type="text" class="form-control" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">From ( Year/ Month )</label>
                                                <div class="col-md-2 col-xs-12">
                                                    <input type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">To ( Year/ Month )</label>
                                                <div class="col-md-2 col-xs-12">
                                                    <input type="text" class="form-control" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-md-3 col-xs-12 control-label">Descrption</label>
                                                <div class="col-md-6 col-xs-12">
                                                    <textarea class="form-control" rows="5"></textarea>
                                                </div>
                                            </div>
                                            <div class="panel-footer">
                                                <button type="button" class="btn btn-primary pull-right btn-save">Save Changes</button>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">

                                                    <!-- START TIMELINE -->
                                                    <div class="timeline">

                                                        <!-- START TIMELINE ITEM -->
                                                        <div class="timeline-item timeline-main">
                                                            <div class="timeline-date">2019</div>
                                                        </div>
                                                        <!-- END TIMELINE ITEM -->

                                                        <!-- START TIMELINE ITEM -->
                                                        <div class="timeline-item">
                                                            <div class="timeline-item-info">2019 Jan - Present</div>
                                                            <div class="timeline-item-icon"><span class="fa fa-globe"></span></div>
                                                            <div class="timeline-item-content">
                                                                <div class="timeline-heading">
                                                                    <img src="assets/images/users/leesons.png" />
                                                                    <a href="#">HR Executive</a> at <a href="#">Leesons Hospital PVT Ltd</a>
                                                                </div>
                                                                <div class="timeline-body">

                                                                    <p>Design compensation and benefits packages</p>
                                                                    <p>Implement performance review procedures</p>
                                                                    <p>Develop fair HR policies and ensure employees understand and comply with them</p>
                                                                    <p>Implement effective sourcing, screening and interviewing techniques</p>
                                                                    <p>Assess training needs and coordinate learning and development initiatives for all employees</p>
                                                                    <p>Monitor HR department’s budget</p>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <!-- END TIMELINE ITEM -->

                                                        <!-- START TIMELINE ITEM -->
                                                        <div class="timeline-item timeline-item-right">
                                                            <div class="timeline-item-info">2015 Jan - 2019 Jan</div>
                                                            <div class="timeline-item-icon"><span class="fa fa-image"></span></div>
                                                            <div class="timeline-item-content">
                                                                <div class="timeline-heading">
                                                                    <img src="assets/images/users/microimage.png" />
                                                                    <a href="#">HR Assistant </a>at <a href="#">HCM MicroImage PVT Ltd</a>

                                                                </div>

                                                                <div class="timeline-body">

                                                                    <p>Manage employees’ grievances</p>
                                                                    <p>Create and run referral bonus programs</p>
                                                                    <p>Review current HR technology and recommend more effective software (including HRIS and ATS)</p>
                                                                    <p>Measure employee retention and turnover rates</p>
                                                                    <p>Oversee daily operations of the HR department</p>
                                                                    <p>Assist with the recruitment process by identifying candidates, performing reference checks, and issuing employment contracts.</p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- END TIMELINE ITEM -->

                                                        <!-- START TIMELINE ITEM -->
                                                        <div class="timeline-item">
                                                            <div class="timeline-item-info">2012 August - 2014 July</div>
                                                            <div class="timeline-item-icon"><span class="fa fa-star"></span></div>
                                                            <div class="timeline-item-content">
                                                                <div class="timeline-heading" style="padding-bottom: 10px;">
                                                                    <img src="assets/images/users/javasolution.png" />
                                                                    <a href="#">Internship in HR Assistant </a>at <a href="#">Java Solution PVt Ltd</a>
                                                                </div>
                                                                <div class="timeline-body">

                                                                    <p>Updating company databases by inputting new employee contact information and employment details.</p>
                                                                    <p>Screening potential employees' resumes and application forms to identify suitable candidates to fill company job vacancies.</p>
                                                                    <p>Organizing interviews with shortlisted candidates.</p>
                                                                    <p>Posting job advertisements to job boards and social media platforms.</p>
                                                                    <p>Removing job advertisements from job boards and social media platforms once vacancies have been filled.</p>
                                                                    <p>Assisting the HR staff in gathering market salary information.</p>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <!-- END TIMELINE ITEM -->

                                                    </div>
                                                    <!-- END TIMELINE -->

                                                </div>
                                            </div>
                                        </div>
                                        <div class="tab-pane" id="tab-seven">

                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Contact Person</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_emContactPerson" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Contact Number</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_emContactNumber" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Adress</label>
                                                        <div class="col-md-6 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_emAddress" runat="server" title="" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-3 col-xs-12 control-label">Relationship</label>
                                                        <div class="col-md-8 col-xs-12">
                                                            <input type="text" class="form-control" value="" id="text_emRelationship" runat="server" title="" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="panel-footer">
                                                <button type="button" class="btn btn-primary pull-right btn-save">Save Changes</button>
                                            </div>

                                        </div>

                                    </div>

                                </div>
                            </panel>
                        </div>
                    </div>
                    <!-- Only change the Save buttons to use the new API -->

                    <!-- Example: Change this in each tab -->


                </div>
            </div>
        </div>
    </form>


    <!-- MESSAGE BOX-->
    <div class="message-box animated fadeIn" data-sound="alert" id="mb-signout">
        <div class="mb-container">
            <div class="mb-middle">
                <div class="mb-title"><span class="fa fa-sign-out"></span>Log <strong>Out</strong> ?</div>
                <div class="mb-content">
                    <p>Are you sure you want to log out?</p>
                    <p>Press No if you want to continue work. Press Yes to logout current user.</p>
                </div>
                <div class="mb-footer">
                    <div class="pull-right">
                        <button class="btn btn-success btn-lg" onclick="performLogout()">Yes</button>
                        <button class="btn btn-default btn-lg mb-control-close">No</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function performLogout() {
            var token = sessionStorage.getItem('AuthToken') || getCookie('RememberToken');
            if (token) {
                $.ajax({
                    url: API_CONFIG.baseUrl + '/auth/logout',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ token: token }),
                    complete: function () {
                        clearAuthData();
                        window.location.href = 'login.aspx';
                    }
                });
            } else {
                clearAuthData();
                window.location.href = 'login.aspx';
            }
        }

        // Set username from session
        $(document).ready(function () {
            var userName = sessionStorage.getItem('UserID') || getCookie('RememberUser') || 'User';
            $('#div_username').text(userName);

            // Get current date in YYYY-MM-DD format (for date inputs)
            const today = new Date();
            const year = today.getFullYear();
            const month = String(today.getMonth() + 1).padStart(2, '0');
            const day = String(today.getDate()).padStart(2, '0');
            const currentDate = `${year}-${month}-${day}`;

            // Set current date for all date inputs
            $('#date_DOB').val(currentDate);
            $('#date_DOA').val(currentDate);
            $('#date_resginedDate').val(currentDate);
        });
    </script>

    <!-- Your existing scripts -->
    <script type='text/javascript' src='js/plugins/icheck/icheck.min.js'></script>
    <script type="text/javascript" src="js/plugins/mcustomscrollbar/jquery.mCustomScrollbar.min.js"></script>
    <script type="text/javascript" src="js/plugins/scrolltotop/scrolltopcontrol.js"></script>
    <script type="text/javascript" src="js/plugins/morris/raphael-min.js"></script>
    <script type="text/javascript" src="js/plugins/morris/morris.min.js"></script>
    <script type="text/javascript" src="js/plugins/rickshaw/d3.v3.js"></script>
    <script type="text/javascript" src="js/plugins/rickshaw/rickshaw.min.js"></script>
    <script type='text/javascript' src='js/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js'></script>
    <script type='text/javascript' src='js/plugins/jvectormap/jquery-jvectormap-world-mill-en.js'></script>
    <script type='text/javascript' src='js/plugins/bootstrap/bootstrap-datepicker.js'></script>
    <script type="text/javascript" src="js/plugins/owl/owl.carousel.min.js"></script>
    <script type="text/javascript" src="js/plugins/moment.min.js"></script>
    <script type="text/javascript" src="js/plugins/daterangepicker/daterangepicker.js"></script>
    <script type="text/javascript" src="js/settings.js"></script>
    <script type="text/javascript" src="js/plugins.js"></script>
    <script type="text/javascript" src="js/actions.js"></script>
</body>
</html>
