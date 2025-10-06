<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="leave.aspx.cs" Inherits="bbq.leave" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <title>HRIS</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="icon" href="favicon.ico" type="image/x-icon" />
    <link href="jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="css/styleloading.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" type="text/css" id="theme" href="css/theme-default.css" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.13.0/jquery-ui.min.js"></script>
    <script type="text/javascript" src="js/bootstrap-min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById("panel_ongoing_values").style.display = "none";
            SearchText();
        });
        function SearchText() {
            $("#text_employee").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "attendance.aspx/getEmployee",
                        data: "{'empName':'" + document.getElementById('text_employee').value + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert(result);
                        }
                    });
                },
                focus: function (event, ui) {

                    return false; // prevent the default behavior of the focus event
                },

                select: function (event, ui) {
                    var result2_ = ui.item.value.split('(')[1];
                    var epfno2_ = result2_.split(',')[0].split('-')[1];
                    $.ajax({
                        beforeSend: function () {
                            $("#loading").css("visibility", "visible");
                        },
                        url: "https://localhost:44341/api/ProcessSalary/CheckEmployee?epfno=" + epfno2_ + "&period=" + document.getElementById('date_search').value + "/" + document.getElementById('list_month').value + "&empid=" + document.getElementById('text_attendanceId').value,

                        success: function (data) {
                            var empState = data

                            $.ajax({
                                type: "POST",
                                url: "leave.aspx/employeeSelected",
                                data: "{'empName':'" + ui.item.value + "','year':'" + document.getElementById('date_search').value + "','month':'" + document.getElementById('list_month').value + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    clear()
                                    if (data.d == '') {

                                        alert('Sorry, Salary not Processed for Selected Employee')
                                        document.getElementById('text_employee').value = ui.item.value.split('(')[0]
                                        $("#loading").css("visibility", "hidden");
                                    } else {
                                        var result = data.d.split('_')
                                        document.getElementById('text_employee').value = ""
                                        document.getElementById('text_epfNo_').value = result[0]
                                        document.getElementById('text_attendanceId').value = result[1]
                                        document.getElementById('text_employeeName').value = result[2]
                                        document.getElementById('text_department').value = result[3]
                                        document.getElementById('text_annual').value = result[4]
                                        document.getElementById('text_cashual').value = result[5]
                                        document.getElementById('text_sick').value = result[6]

                                        var result = ui.item.value.split('(')[1];
                                        var epfno = result.split(',')[0].split('-')[1]

                                        loadLeaves(epfno, empState)
                                    }
                                },
                                error: function (result) {

                                }
                            });


                        },
                        complete: function () {

                        }
                    });


                }
            }

            );
        }
    </script>
    <script type="text/javascript">
        function clear() {
            document.getElementById('text_epfNo_').value = ""
            document.getElementById('text_attendanceId').value = ""
            document.getElementById('text_employeeName').value = ""
            document.getElementById('text_department').value = ""
            document.getElementById('text_annual').value = ""
            document.getElementById('text_cashual').value = ""
            document.getElementById('text_sick').value = ""


            var table = $("#table tbody");
            table.empty();
        }

    </script>
    <script type="text/javascript">
        function loadLeaves(epfno, empState) {

        }
    </script>
</head>
<body>
    <!-- START PAGE CONTAINER -->
    <form runat="server">
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
                            <img src="assets/images/users/avatar.png" alt="John Doe" />
                        </a>
                        <div class="profile">
                            <div class="profile-image">
                                <img src="assets/images/users/avatar.png" alt="John Doe" />
                            </div>
                            <div class="profile-data">
                                <div class="profile-data-name">Sadali Perera</div>
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
                    <li class="xn-openable">
                        <a href="#"><span class="fa fa-files-o"></span><span class="xn-text">Profiles</span></a>
                        <ul>
                            <li><a href="profile_employee.aspx"><span class="fa fa-image"></span>Employee Master</a></li>
                            <li><a href="profile_users.aspx"><span class="fa fa-user"></span>Users</a></li>

                        </ul>
                    </li>
                    <li>
                        <a href="payroll.aspx"><span class="fa fa-file-text-o"></span><span class="xn-text">Payroll</span></a>

                    </li>
                    <li class="xn-openable ">
                        <a href="#"><span class="fa fa-file-text-o"></span><span class="xn-text">Time & Attendance</span></a>
                        <ul>
                            <li><a href="attendance.aspx"><span class="fa fa-heart"></span>Attendance</a></li>
                            <li><a href="roaster.aspx"><span class="fa fa-cogs"></span>Roaster</a></li>
                        </ul>
                    </li>
                    <li class="xn-openable active">
                        <a href="#"><span class="fa fa-cogs"></span><span class="xn-text">On-Going Function</span></a>
                        <ul>
                            <li><a href="deduction.aspx"><span class="fa fa-heart"></span>Deductions & Allowances</a></li>
                            <li class="active"><a href="leave.aspx"><span class="fa fa-square-o"></span>Leave</a></li>

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
                        <a href="tables.html"><span class="fa fa-table"></span><span class="xn-text">Settings</span></a>
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
                    <!-- SEARCH -->
                    <li class="xn-search">
                        <form role="form">
                            <input type="text" name="search" placeholder="Search..." />
                        </form>
                    </li>
                    <!-- END SEARCH -->
                    <!-- SIGN OUT -->
                    <li class="xn-icon-button pull-right">
                        <a href="#" class="mb-control" data-box="#mb-signout"><span class="fa fa-sign-out"></span></a>
                    </li>
                    <!-- END SIGN OUT -->
                    <!-- MESSAGES -->
                    <li class="xn-icon-button pull-right">
                        <a href="#"><span class="fa fa-comments"></span></a>
                        <div class="informer informer-danger">4</div>
                        <div class="panel panel-primary animated zoomIn xn-drop-left xn-panel-dragging">
                            <div class="panel-heading">
                                <h3 class="panel-title"><span class="fa fa-comments"></span>Messages</h3>
                                <div class="pull-right">
                                    <span class="label label-danger">4 new</span>
                                </div>
                            </div>
                            <div class="panel-body list-group list-group-contacts scroll" style="height: 200px;">
                                <a href="#" class="list-group-item">
                                    <div class="list-group-status status-online"></div>
                                    <img src="assets/images/users/user2.jpg" class="pull-left" alt="John Doe" />
                                    <span class="contacts-title">John Doe</span>
                                    <p>Praesent placerat tellus id augue condimentum</p>
                                </a>
                                <a href="#" class="list-group-item">
                                    <div class="list-group-status status-away"></div>
                                    <img src="assets/images/users/user.jpg" class="pull-left" alt="Dmitry Ivaniuk" />
                                    <span class="contacts-title">Dmitry Ivaniuk</span>
                                    <p>Donec risus sapien, sagittis et magna quis</p>
                                </a>
                                <a href="#" class="list-group-item">
                                    <div class="list-group-status status-away"></div>
                                    <img src="assets/images/users/user3.jpg" class="pull-left" alt="Nadia Ali" />
                                    <span class="contacts-title">Nadia Ali</span>
                                    <p>Mauris vel eros ut nunc rhoncus cursus sed</p>
                                </a>
                                <a href="#" class="list-group-item">
                                    <div class="list-group-status status-offline"></div>
                                    <img src="assets/images/users/user6.jpg" class="pull-left" alt="Darth Vader" />
                                    <span class="contacts-title">Darth Vader</span>
                                    <p>I want my money back!</p>
                                </a>
                            </div>
                            <div class="panel-footer text-center">
                                <a href="pages-messages.html">Show all messages</a>
                            </div>
                        </div>
                    </li>
                    <!-- END MESSAGES -->
                    <!-- TASKS -->
                    <li class="xn-icon-button pull-right">
                        <a href="#"><span class="fa fa-tasks"></span></a>
                        <div class="informer informer-warning">3</div>
                        <div class="panel panel-primary animated zoomIn xn-drop-left xn-panel-dragging">
                            <div class="panel-heading">
                                <h3 class="panel-title"><span class="fa fa-tasks"></span>Tasks</h3>
                                <div class="pull-right">
                                    <span class="label label-warning">3 active</span>
                                </div>
                            </div>
                            <div class="panel-body list-group scroll" style="height: 200px;">
                                <a class="list-group-item" href="#">
                                    <strong>Phasellus augue arcu, elementum</strong>
                                    <div class="progress progress-small progress-striped active">
                                        <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100" style="width: 50%;">50%</div>
                                    </div>
                                    <small class="text-muted">John Doe, 25 Sep 2014 / 50%</small>
                                </a>
                                <a class="list-group-item" href="#">
                                    <strong>Aenean ac cursus</strong>
                                    <div class="progress progress-small progress-striped active">
                                        <div class="progress-bar progress-bar-warning" role="progressbar" aria-valuenow="80" aria-valuemin="0" aria-valuemax="100" style="width: 80%;">80%</div>
                                    </div>
                                    <small class="text-muted">Dmitry Ivaniuk, 24 Sep 2014 / 80%</small>
                                </a>
                                <a class="list-group-item" href="#">
                                    <strong>Lorem ipsum dolor</strong>
                                    <div class="progress progress-small progress-striped active">
                                        <div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="95" aria-valuemin="0" aria-valuemax="100" style="width: 95%;">95%</div>
                                    </div>
                                    <small class="text-muted">John Doe, 23 Sep 2014 / 95%</small>
                                </a>
                                <a class="list-group-item" href="#">
                                    <strong>Cras suscipit ac quam at tincidunt.</strong>
                                    <div class="progress progress-small">
                                        <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%;">100%</div>
                                    </div>
                                    <small class="text-muted">John Doe, 21 Sep 2014 /</small><small class="text-success"> Done</small>
                                </a>
                            </div>
                            <div class="panel-footer text-center">
                                <a href="pages-tasks.html">Show all tasks</a>
                            </div>
                        </div>
                    </li>
                    <!-- END TASKS -->
                </ul>
                <!-- END X-NAVIGATION VERTICAL -->

                <!-- START BREADCRUMB -->
                <ul class="breadcrumb">
                    <li><a href="home.aspx">Dashboard</a></li>
                    <li class="active">Leave</li>
                </ul>
                <!-- END BREADCRUMB -->

                <!-- PAGE CONTENT WRAPPER -->
                <div class="page-content-wrap">

                    <div class="row">
                        <div class="col-md-12">

                            <div class="block">

                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <asp:DropDownList runat="server" ID="date_search" CssClass="form-control" Width="100px" onchange="myFunctionDate()">
                                        </asp:DropDownList>
                                        <script type="text/javascript">
                                            function myFunctionDate() {
                                                if (document.getElementById('text_epfNo_').value == "") {

                                                } else {
                                                    $("#loading").css("visibility", "visible");
                                                    document.getElementById("panel_ongoing_values").style.display = "none";
                                                    var epfno2_ = document.getElementById('text_epfNo_').value
                                                    loadEmployee(epfno2_, "0");
                                                }
                                            }
                                        </script>

                                    </div>

                                </div>
                                <div class="col-md-2">
                                    <div class="form-group has-success">

                                        <select class="form-control select" data-style="btn-success" id="list_month" onchange="myFunction()" runat="server">
                                            <option>January</option>
                                            <option>February</option>
                                            <option>March</option>
                                            <option>April</option>
                                            <option>May</option>
                                            <option>June</option>
                                            <option>July</option>
                                            <option>August</option>
                                            <option>September</option>
                                            <option>Octomber</option>
                                            <option>November</option>
                                            <option>December</option>
                                        </select>
                                        <script>
                                            function myFunction() {
                                                if (document.getElementById('text_epfNo_').value == "") {

                                                } else {
                                                    $("#loading").css("visibility", "visible");
                                                    document.getElementById("panel_ongoing_values").style.display = "none";
                                                    var epfno2_ = document.getElementById('text_epfNo_').value
                                                    loadEmployee(epfno2_, "0");

                                                }
                                            }
                                        </script>
                                    </div>

                                </div>
                                <div class="col-md-4">
                                    <div class="form-group has-success">
                                        <asp:TextBox ID="text_employee" runat="server" CssClass="form-control" Text="" Placeholder="Search Employee here...." />
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">

                            <div class="block">
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">EPF No</label>
                                        <input type="text" class="form-control" value="" id="text_epfNo_" runat="server" readonly style="color: Green" />
                                    </div>

                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">Employee No</label>
                                        <input type="text" class="form-control" value="" id="text_attendanceId" runat="server" readonly style="color: Green" />
                                    </div>

                                </div>
                                <div class="col-md-3">
                                    <div class="form-group has-success">
                                        <label class="control-label">Employee Name</label>
                                        <input type="text" class="form-control" value="" id="text_employeeName" runat="server" readonly style="color: Green" />
                                    </div>

                                </div>
                                <div class="col-md-2">
                                    <div class="form-group has-success">
                                        <label class="control-label">Department</label>
                                        <input type="text" class="form-control" value="" id="text_department" runat="server" readonly style="color: Green" />
                                    </div>

                                </div>

                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">Annual Leave Balance</label>
                                        <asp:TextBox ID="text_annual" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                    </div>

                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">Cashual Leave Balance</label>
                                        <asp:TextBox ID="text_cashual" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                    </div>

                                </div>
                                <div class="col-md-1">
                                    <div class="form-group has-success">
                                        <label class="control-label">Sick Leave Balance</label>
                                        <asp:TextBox ID="text_sick" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-7">


                            <panel class="form-horizontal">

                                <div class="panel panel-default">
                                    <div class="panel panel-heading">
                                        Online Requests
                                    </div>
                                    <div class="panel-body">

                                        <table class="table">
                                            <thead>
                                                <tr>

                                                    <th>Requseted</th>
                                                    <th>Leave Type</th>
                                                    <th>Remark</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>

                                </div>

                            </panel>
                        </div>
                        <div class="col-md-5">


                            <panel class="form-horizontal">

                                <div class="panel panel-default">
                                    <div class="panel panel-heading">
                                        Manual Update
                                    </div>
                                    <div class="panel-body">

                                        <div class="panel panel-default">



                                            <div class="panel-body ">
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Leave Type</label>
                                                    <div class="col-md-5">
                                                        <select class="form-control select" data-style="btn-success" id="list_type" runat="server">
                                                            <option>Annual</option>
                                                            <option>Cashual</option>
                                                            <option>Sick</option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Date</label>
                                                    <div class="col-md-5">
                                                        <input id="date_manual" runat="server" type="text" class="form-control datepicker" value="2023-01-04">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Remark</label>
                                                    <div class="col-md-7">
                                                        <input id="text_remark" runat="server" placeholder="type here" type="text" class="form-control ">
                                                    </div>
                                                </div>


                                            </div>
                                            <div class="panel-footer">

                                                <div>
                                                    <button class="btn btn-primary pull-right">Save<span class="fa fa-floppy-o fa-right"></span></button>

                                                </div>


                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </panel>
                        </div>

                    </div>

                </div>

                <!-- END PAGE CONTENT WRAPPER -->
            </div>
            <!-- END PAGE CONTENT -->
        </div>
        <!-- END PAGE CONTAINER -->

        <!-- MESSAGE BOX-->
        <div class="message-box animated fadeIn" data-sound="alert" id="mb-signout">
            <div class="mb-container">
                <div class="mb-middle">
                    <div class="mb-title"><span class="fa fa-sign-out"></span>Log <strong>Out</strong> ?</div>
                    <div class="mb-content">
                        <p>Are you sure you want to log out?</p>
                        <p>Press No if youwant to continue work. Press Yes to logout current user.</p>
                    </div>
                    <div class="mb-footer">
                        <div class="pull-right">
                            <a href="pages-login.html" class="btn btn-success btn-lg">Yes</a>
                            <button class="btn btn-default btn-lg mb-control-close">No</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END MESSAGE BOX-->

        <!-- START PRELOADS -->
        <audio id="audio-alert" src="audio/alert.mp3" preload="auto"></audio>
        <audio id="audio-fail" src="audio/fail.mp3" preload="auto"></audio>
        <!-- END PRELOADS -->

        <!-- START SCRIPTS -->
        <!-- START PLUGINS -->
        <script type="text/javascript" src="js/plugins/jquery/jquery.min.js"></script>
        <script type="text/javascript" src="js/plugins/jquery/jquery-ui.min.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap.min.js"></script>
        <!-- END PLUGINS -->

        <!-- THIS PAGE PLUGINS -->
        <script type='text/javascript' src='js/plugins/icheck/icheck.min.js'></script>
        <script type="text/javascript" src="js/plugins/mcustomscrollbar/jquery.mCustomScrollbar.min.js"></script>

        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap-datepicker.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap-timepicker.min.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap-colorpicker.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap-file-input.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap-select.js"></script>
        <script type="text/javascript" src="js/plugins/tagsinput/jquery.tagsinput.min.js"></script>
        <!-- END THIS PAGE PLUGINS -->


        <!-- START TEMPLATE -->
        <script type="text/javascript" src="js/settings.js"></script>
        <script>
            nload = function () {

                // the date/time being edited
                var theDate = new Date();

                // create InputDate control
                var inputDate = new wijmo.input.InputDate('#theInputDate', {
                    min: new Date(2014, 8, 1),
                    format: 'yyyy',
                    selectionMode: 2,
                    value: theDate,
                    isDroppedDownChanged: function (s, e) {
                        setTimeout(function () {
                            var _hdr = s.dropDown.querySelector(".wj-calendar-year tr.wj-header td");
                            console.log(_hdr)
                            _hdr.click();
                        }, 100)
                    }
                });
            }
        </script>
        <script type="text/javascript" src="js/plugins.js"></script>
        <script type="text/javascript" src="js/actions.js"></script>
    </form>
</body>
</html>

