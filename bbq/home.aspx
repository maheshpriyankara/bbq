<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="bbq.home" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <title>HRIS</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="icon" href="favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" id="theme" href="css/theme-default.css" />

    <link href="jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="css/styleloading.css" rel="stylesheet" type="text/css" />

    <!-- Include jQuery ONLY ONCE at the top -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

    <style type="text/css">
        .hidden {
            display: none;
        }
        /* Loading indicator */
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
    </style>
</head>
<body>
    <!-- Loading indicator -->
    <div id="loading" class="hidden">
        <div class="spinner"></div>
    </div>

    <div class="page-container">
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
                            <div class="profile-data-name" id="div_username" runat="server">Sadali Perera</div>
                            <div class="profile-data-title" id="div_position" runat="server">HR Admin</div>
                        </div>
                        <div class="profile-controls">
                            <a href="pages-profile.html" class="profile-control-left"><span class="fa fa-info"></span></a>
                            <a href="pages-messages.html" class="profile-control-right"><span class="fa fa-envelope"></span></a>
                        </div>
                    </div>
                </li>
                <li class="active" id="menu" runat="server">
                    <a href="home.aspx"><span class="fa fa-desktop"></span><span class="xn-text">Dashboard</span></a>
                </li>
                <li class="xn-openable" id="menu2" runat="server">
                    <a href="#"><span class="fa fa-files-o"></span><span class="xn-text">Profiles</span></a>
                    <ul>
                        <li><a href="profile_employee.aspx"><span class="fa fa-image"></span>Employee Master</a></li>
                    </ul>
                </li>
                <li id="menu3" runat="server">
                    <a href="payroll.aspx"><span class="fa fa-file-text-o"></span><span class="xn-text">Payroll</span></a>
                </li>
                <li class="xn-openable" id="menu4" runat="server">
                    <a href="#"><span class="fa fa-file-text-o"></span><span class="xn-text">Time & Attendance</span></a>
                    <ul>
                        <li><a href="attendance.aspx"><span class="fa fa-heart"></span>Attendance</a></li>
                        <li><a href="roaster.aspx"><span class="fa fa-cogs"></span>Roaster</a></li>
                    </ul>
                </li>
                <li class="xn-openable" id="menu5" runat="server">
                    <a href="#"><span class="fa fa-cogs"></span><span class="xn-text">On-Going Function</span></a>
                    <ul>
                        <li><a href="deduction.aspx"><span class="fa fa-heart"></span>Deductions & Allowances</a></li>
                        <li><a href="leave.aspx"><span class="fa fa-square-o"></span>Leave</a></li>
                    </ul>
                </li>
                <li class="xn-openable" id="menu6" runat="server">
                    <a href="#"><span class="fa fa-pencil"></span><span class="xn-text">Reports</span></a>
                    <ul>
                        <li><a href="report_salary_summary.aspx"><span class="fa fa-file-text-o"></span>Salary Summary (Employee)</a></li>
                        <li><a href="report_employee.aspx"><span class="fa fa-file-text-o"></span>Employee Report</a></li>
                        <li><a href="advance_reports.aspx"><span class="fa fa-file-text-o"></span>Other Reports</a></li>
                    </ul>
                </li>
                <li class="xn-openable">
                    <a href="#"><span class="fa fa-bitbucket"></span><span class="xn-text">Recruitment</span></a>
                    <ul>
                        <li><a href="job_posting.aspx"><span class="fa fa-heart"></span>Job Posting</a></li>
                        <li><a href="applicant_dashboard.aspx"><span class="fa fa-square-o"></span>Applicant DashBoard</a></li>
                        <li><a href="interview.aspx"><span class="fa fa-square-o"></span>Interview</a></li>
                        <li><a href="onboarding.aspx"><span class="fa fa-square-o"></span>Onboarding</a></li>
                    </ul>
                </li>
                <li class="xn-openable" id="menu7" runat="server">
                    <a href="tables.html"><span class="fa fa-table"></span><span class="xn-text">Settings</span></a>
                    <ul>
                        <li><a href="settings_.aspx"><span class="fa fa-align-justify"></span>Settings</a></li>
                    </ul>
                </li>
            </ul>
            <!-- END X-NAVIGATION -->
        </div>
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
                <li><a href="#">Home</a></li>
                <li class="active">Dashboard</li>
            </ul>
            <!-- END BREADCRUMB -->

            <!-- PAGE CONTENT WRAPPER -->
            <div class="page-content-wrap">

                <!-- START WIDGETS -->
                <div class="row">
                    <div class="col-md-3">

                        <!-- START WIDGET SLIDER -->
                        <div class="widget widget-default widget-carousel">
                            <div class="owl-carousel" id="owl-example">
                                <div>
                                    <div class="widget-title">Total Present</div>
                                    <div class="widget-subtitle">Today</div>
                                    <div class="widget-int" id="text_present" runat="server"></div>
                                </div>
                                <div>
                                    <div class="widget-title">Total Absent</div>
                                    <div class="widget-subtitle">Today</div>
                                    <div class="widget-int" id="text_absent" runat="server"></div>
                                </div>
                                <div>
                                    <div class="widget-title">New Commers</div>
                                    <div class="widget-subtitle">Today</div>
                                    <div class="widget-int" id="text_newcomers" runat="server"></div>
                                </div>
                            </div>
                            <div class="widget-controls">
                                <a href="#" class="widget-control-right widget-remove" data-toggle="tooltip" data-placement="top" title="Remove Widget"><span class="fa fa-times"></span></a>
                            </div>
                        </div>
                        <!-- END WIDGET SLIDER -->

                    </div>
                    <div class="col-md-2">

                        <!-- START WIDGET MESSAGES -->
                        <div class="widget widget-default widget-item-icon" onclick="location.href='pages-messages.html';">
                            <div class="widget-item-left">
                                <span class="fa fa-envelope"></span>
                            </div>
                            <div class="widget-data">
                                <div class="widget-int num-count">48</div>
                                <div class="widget-title"><a href="massages.aspx">New messages</a></div>
                            </div>
                            <div class="widget-controls">
                                <a href="#" class="widget-control-right widget-remove" data-toggle="tooltip" data-placement="top" title="Remove Widget"><span class="fa fa-times"></span></a>
                            </div>
                        </div>
                        <!-- END WIDGET MESSAGES -->

                    </div>
                    <div class="col-md-2">

                        <!-- START WIDGET REGISTRED -->
                        <div class="widget widget-default widget-item-icon" onclick="location.href='pages-address-book.html';">
                            <div class="widget-item-left">
                                <span class="fa fa-user"></span>
                            </div>
                            <div class="widget-data">
                                <div class="widget-int num-count">18</div>
                                <div class="widget-title"><a href="leave_authorization.aspx">Leave Requsets</a></div>
                            </div>
                            <div class="widget-controls">
                                <a href="#" class="widget-control-right widget-remove" data-toggle="tooltip" data-placement="top" title="Remove Widget"><span class="fa fa-times"></span></a>
                            </div>
                        </div>
                        <!-- END WIDGET REGISTRED -->

                    </div>
                    <div class="col-md-2">

                        <!-- START WIDGET REGISTRED -->
                        <div class="widget widget-default widget-item-icon" onclick="location.href='pages-address-book.html';">
                            <div class="widget-item-left">
                                <span class="fa fa-user"></span>
                            </div>
                            <div class="widget-data">
                                <div class="widget-int num-count">10</div>
                                <div class="widget-title"><a href="advance_authorization.aspx">Advance Requsets</a></div>
                            </div>
                            <div class="widget-controls">
                                <a href="#" class="widget-control-right widget-remove" data-toggle="tooltip" data-placement="top" title="Remove Widget"><span class="fa fa-times"></span></a>
                            </div>
                        </div>
                        <!-- END WIDGET REGISTRED -->

                    </div>
                    <div class="col-md-3">

                        <!-- START WIDGET CLOCK -->
                        <div class="widget widget-info widget-padding-sm">
                            <div class="widget-big-int plugin-clock">00:00</div>
                            <div class="widget-subtitle plugin-date">Loading...</div>
                            <div class="widget-controls">
                                <a href="#" class="widget-control-right widget-remove" data-toggle="tooltip" data-placement="left" title="Remove Widget"><span class="fa fa-times"></span></a>
                            </div>
                            <div class="widget-buttons widget-c3">
                                <div class="col">
                                    <a href="#"><span class="fa fa-clock-o"></span></a>
                                </div>
                                <div class="col">
                                    <a href="#"><span class="fa fa-bell"></span></a>
                                </div>
                                <div class="col">
                                    <a href="#"><span class="fa fa-calendar"></span></a>
                                </div>
                            </div>
                        </div>
                        <!-- END WIDGET CLOCK -->

                    </div>
                </div>
                <!-- END WIDGETS -->

                <div class="row">
                    <div class="col-md-4">

                        <!-- START USERS ACTIVITY BLOCK -->
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="panel-title-box">
                                    <h3>Attendance ( Last Week )</h3>
                                    <span>Present vs Absent</span>
                                </div>
                                <ul class="panel-controls" style="margin-top: 2px;">
                                    <li><a href="#" class="panel-fullscreen"><span class="fa fa-expand"></span></a></li>
                                    <li><a href="#" class="panel-refresh"><span class="fa fa-refresh"></span></a></li>
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown"><span class="fa fa-cog"></span></a>
                                        <ul class="dropdown-menu">
                                            <li><a href="#" class="panel-collapse"><span class="fa fa-angle-down"></span>Collapse</a></li>
                                            <li><a href="#" class="panel-remove"><span class="fa fa-times"></span>Remove</a></li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                            <div class="panel-body padding-0">
                                <div class="chart-holder" id="dashboard-bar-1" style="height: 200px;"></div>
                            </div>
                        </div>
                        <!-- END USERS ACTIVITY BLOCK -->

                    </div>
                    <div class="col-md-4">

                        <!-- START VISITORS BLOCK -->
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="panel-title-box">
                                    <h3>Present Summary</h3>
                                    <span>Today</span>
                                </div>
                                <ul class="panel-controls" style="margin-top: 2px;">
                                    <li><a href="#" class="panel-fullscreen"><span class="fa fa-expand"></span></a></li>
                                    <li><a href="#" class="panel-refresh"><span class="fa fa-refresh"></span></a></li>
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown"><span class="fa fa-cog"></span></a>
                                        <ul class="dropdown-menu">
                                            <li><a href="#" class="panel-collapse"><span class="fa fa-angle-down"></span>Collapse</a></li>
                                            <li><a href="#" class="panel-remove"><span class="fa fa-times"></span>Remove</a></li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                            <div class="panel-body padding-0">
                                <div class="chart-holder" id="dashboard-donut-1" style="height: 200px;"></div>
                            </div>
                        </div>
                        <!-- END VISITORS BLOCK -->

                    </div>

                    <div class="col-md-4">

                        <!-- START PROJECTS BLOCK -->
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="panel-title-box">
                                    <h3>Absent Report</h3>
                                    <span>Today</span>
                                </div>
                            </div>
                            <div class="panel-body panel-body-table">

                                <div class="table-responsive" style="max-height: 400px; overflow-y: auto;">
                                    <table class="tabl" id="table" runat="server">
                                        <thead>
                                            <tr>
                                                <th width="50%"></th>
                                                <th width="20%"></th>
                                                <th width="30%"></th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>

                            </div>
                        </div>
                        <!-- END PROJECTS BLOCK -->

                    </div>
                </div>

                <div class="row">


                    <div class="col-md-12">

                        <!-- START SALES & EVENTS BLOCK -->
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="panel-title-box">
                                    <h3>Overtime History ( Hours )</h3>
                                    <span>Last Week</span>
                                </div>
                                <ul class="panel-controls" style="margin-top: 2px;">
                                    <li><a href="#" class="panel-fullscreen"><span class="fa fa-expand"></span></a></li>
                                    <li><a href="#" class="panel-refresh"><span class="fa fa-refresh"></span></a></li>
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown"><span class="fa fa-cog"></span></a>
                                        <ul class="dropdown-menu">
                                            <li><a href="#" class="panel-collapse"><span class="fa fa-angle-down"></span>Collapse</a></li>
                                            <li><a href="#" class="panel-remove"><span class="fa fa-times"></span>Remove</a></li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                            <div class="panel-body padding-0">
                                <div id="morris-line-example" style="height: 300px;"></div>
                            </div>
                        </div>


                        <!-- END SALES & EVENTS BLOCK -->

                    </div>
                </div>
                <div class="row" runat="server">
                    <div class="col-md-5" id="panel_processqueue">

                        <!-- START PROJECTS BLOCK -->
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="panel-title-box">
                                    <h3>Process Queue (Finle {user})</h3>
                                    <span>real-time</span>
                                </div>
                            </div>
                            <div class="panel-body panel-body-table">

                                <div class="table-responsive" style="max-height: 400px; overflow-y: auto;">
                                    <table class="tabl" id="table2" runat="server">
                                        <thead>
                                            <tr>
                                                <th width="50%"></th>
                                                <th width="20%"></th>
                                                <th width="30%"></th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>

                            </div>
                        </div>
                        <!-- END PROJECTS BLOCK -->

                    </div>
                    <div class="col-md-5" id="panel_processqueue2">

                        <!-- START PROJECTS BLOCK -->
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="panel-title-box">
                                    <h3>Process Queue (TimeSheet-Single Day Requets {user})</h3>
                                    <span>real-time</span>
                                </div>
                            </div>
                            <div class="panel-body panel-body-table">

                                <div class="table-responsive" style="max-height: 400px; overflow-y: auto;">
                                    <table class="tabl" id="table3" runat="server">
                                        <thead>
                                            <tr>
                                                <th width="50%"></th>
                                                <th width="20%"></th>
                                                <th width="30%"></th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>

                            </div>
                        </div>
                        <!-- END PROJECTS BLOCK -->

                    </div>
                    <div class="col-md-5" id="panel_processqueue3">

                        <!-- START PROJECTS BLOCK -->
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="panel-title-box">
                                    <h3>Process Queue (TimeSheet-Monthly Process {auto})</h3>
                                    <span>real-time</span>
                                </div>
                            </div>
                            <div class="panel-body panel-body-table">

                                <div class="table-responsive" style="max-height: 400px; overflow-y: auto;">
                                    <table class="tabl" id="table4" runat="server">
                                        <thead>
                                            <tr>
                                                <th width="50%"></th>
                                                <th width="20%"></th>
                                                <th width="30%"></th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>

                            </div>
                        </div>
                        <!-- END PROJECTS BLOCK -->

                    </div>
                </div>
                <!-- START DASHBOARD CHART -->
                <div class="chart-holder" id="dashboard-area-1" style="height: 200px;"></div>
                <div class="block-full-width">
                </div>
                <!-- END DASHBOARD CHART -->

            </div>
            <!-- END PAGE CONTENT WRAPPER -->
        </div>
        <!-- END PAGE CONTENT -->
    </div>
    <!-- END PAGE CONTAINER -->

    <!-- SINGLE MESSAGE BOX FOR LOGOUT -->
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
                        <button class="btn btn-success btn-lg" id="btnLogout">Yes</button>
                        <button class="btn btn-default btn-lg mb-control-close">No</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="js/bootstrap-min.js"></script>
    <!-- END PLUGINS -->

    <!-- START THIS PAGE PLUGINS-->
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
    <!-- END THIS PAGE PLUGINS-->

    <!-- START TEMPLATE -->
    <script type="text/javascript" src="js/settings.js"></script>
    <script type="text/javascript" src="js/plugins.js"></script>
    <script type="text/javascript" src="js/actions.js"></script>
    <script type="text/javascript" src="js/demo_charts_morris.js"></script>
    <script type="text/javascript" src="js/demo_dashboard.js"></script>
    <!-- END TEMPLATE -->

    <script type="text/javascript">
        let inactivityTimer;
        const API_CONFIG = {
            baseUrl: 'https://localhost:44341/api',
            endpoints: {
                employees: '/employees',
                search: '/employees/search',
                validateToken: '/auth/validatetoken',
                masters: {
                    companies: '/masters/companies',
                    designations: '/masters/designations',
                    departments: '/masters/departments',
                    shiftblocks: '/masters/shiftblocks'
                }
            }
        };
        function fetchCompanySettings(token) {
           
            // Otherwise fetch from user-settings endpoint
            $.ajax({
                url: API_CONFIG.baseUrl + '/home/company-settings?token=' + encodeURIComponent(token),
                type: 'GET',
                success: function (settingsResponse) {
                    if (settingsResponse.success) {


                    } else {
                        showMessageError('Please Refresh the Page', 'Company Settings Loading error')
                    }
                },
                error: function (xhr, status, error) {
                    showMessageError('Please Refresh the Page', 'Company Settings Loading error');
                }
            });
        }

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
            initializeApplication();
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
        }

        function logoutDueToInactivity() {
            const inactivityTimeout = parseInt(sessionStorage.getItem('inactivityTimeout')) || 5;

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

                    validateToken(token);
                    return;
                } else {
                    console.log('No active session found, redirecting to login');
                    //  window.location.href = 'login.aspx';
                    return;
                }
            }

            // For remember me users returning, always give fresh start
            if (getCookie('RememberToken')) {
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
                    //  window.location.href = 'login.aspx';
                    return;
                }
            }

            validateToken(token);
        }

        function validateToken(token) {
            $.ajax({
                url: 'https://localhost:44341/api/auth/validatetoken',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ token: token }),
                success: function (response) {
                    if (response && response.valid) {
                        loadDashboard();
                        fetchCompanySettings(token);
                    } else {
                        console.log('Token invalid, redirecting to login');
                        clearAuthData();
                        //  window.location.href = 'login.aspx';
                    }
                },
                error: function (xhr, status, error) {
                    console.log('Token validation error:', error);
                    clearAuthData();
                    //   window.location.href = 'login.aspx';
                }
            });
        }

        function loadDashboard() {
            var userName = sessionStorage.getItem('UserID') || getCookie('RememberUser') || 'User';
            $('#div_username').text(userName);

            // Get and display user's inactivity timeout
            var inactivityTimeout = parseInt(sessionStorage.getItem('inactivityTimeout')) || 5;

            // Reset inactivity timer when dashboard loads
            resetInactivityTimer();

            // Your existing dashboard loading functions
            //myMethod();
            //myMethod2();
            //myMethod3();
            //myMethod4();
            //myMethod5();

            //setInterval(myMethod, 5000);
            //setInterval(myMethod2, 5000);
            //setInterval(myMethod3, 1000);
            //setInterval(myMethod4, 1000);
            //setInterval(myMethod5, 1000);
        }

        function performLogout() {
            // Clear the inactivity timer when user manually logs out
            if (inactivityTimer) {
                clearTimeout(inactivityTimer);
            }
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
                        //  window.location.href = 'login.aspx';
                    },
                    error: function () {
                        clearAuthData();
                        //       window.location.href = 'login.aspx';
                    }
                });
            } else {
                clearAuthData();
                //  window.location.href = 'login.aspx';
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

        // Your existing methods (keep them exactly as they are)
        function myMethod() {
            $.ajax({
                beforeSend: function () {
                    $("#loading").removeClass("hidden");
                },
                url: "https://localhost:44341/api/attendance/GetDayAttendance",
                success: function (data) {
                    $.each(data, function (index, value) {
                        document.getElementById('text_present').innerText = value.Present
                        document.getElementById('text_absent').innerText = value.Absent
                        document.getElementById('text_newcomers').innerText = value.NewComers
                    });
                },
                complete: function () {
                    $("#loading").addClass("hidden");
                }
            });
        }

        function myMethod2() {
            $.ajax({
                beforeSend: function () {
                    $("#loading").removeClass("hidden");
                },
                url: "https://localhost:44341/api/attendance/GetDayAttendanceLine",
                success: function (data) {
                    var table = $("#table tbody");
                    table.empty();

                    $.each(data, function (index, value) {
                        var row = $("<tr>");
                        row.append($("<td>").text(value.Name));
                        row.append($("<td>").text(value.Count + " Employee's"));
                        if (value.Status == 'Completed') {
                            var statusColumn = $("<td>").text(value.Status);
                            statusColumn.addClass("label label-success");
                            row.append(statusColumn);
                        } else {
                            var statusColumn = $("<td>").text(value.Status);
                            statusColumn.addClass("label label-danger");
                            row.append(statusColumn);
                        }
                        table.append(row);
                    });
                },
                complete: function () {
                    $("#loading").addClass("hidden");
                }
            });
        }

        function myMethod3() {
            $.ajax({
                beforeSend: function () {
                    $("#loading").removeClass("hidden");
                },
                url: "https://localhost:44341/api/ProcessSalary/getProcessQueue",
                success: function (data) {
                    var table2 = $("#table2 tbody");
                    table2.empty();
                    if (data.length > 0) {
                        var lastRow = data[data.length - 1];
                        var firstRow = $("<tr>");
                        firstRow.append($("<td>").text(lastRow.Period));
                        firstRow.append($("<td>").text(lastRow.Line));
                        firstRow.append($("<td>").text(lastRow.Count));
                        table2.prepend(firstRow);

                        $.each(data.slice(0, data.length - 1), function (index, value) {
                            var row = $("<tr>");
                            row.append($("<td>").text(value.Period));
                            row.append($("<td>").text(value.Line));
                            row.append($("<td>").text(value.Count));
                            table2.append(row);
                        });
                    }
                },
                complete: function () {
                    $("#loading").addClass("hidden");
                }
            });
        }

        function myMethod4() {
            $.ajax({
                beforeSend: function () {
                    $("#loading").removeClass("hidden");
                },
                url: "https://localhost:44341/api/ProcessSalary/getProcessQueueTimesheet",
                success: function (data) {
                    var table3 = $("#table3 tbody");
                    table3.empty();
                    if (data.length > 0) {
                        var lastRow = data[data.length - 1];
                        var firstRow = $("<tr>");
                        firstRow.append($("<td>").text(lastRow.Period));
                        firstRow.append($("<td>").text(lastRow.Line));
                        firstRow.append($("<td>").text(lastRow.Count));
                        table3.prepend(firstRow);

                        $.each(data.slice(0, data.length - 1), function (index, value) {
                            var row = $("<tr>");
                            row.append($("<td>").text(value.Period));
                            row.append($("<td>").text(value.Line));
                            row.append($("<td>").text(value.Count));
                            table3.append(row);
                        });
                    }
                },
                complete: function () {
                    $("#loading").addClass("hidden");
                }
            });
        }

        function myMethod5() {
            $.ajax({
                beforeSend: function () {
                    $("#loading").removeClass("hidden");
                },
                url: "https://localhost:44341/api/ProcessSalary/getProcessQueueTimesheetMonth",
                success: function (data) {
                    var table4 = $("#table4 tbody");
                    table4.empty();
                    if (data.length > 0) {
                        var lastRow = data[data.length - 1];
                        var firstRow = $("<tr>");
                        firstRow.append($("<td>").text(lastRow.Period));
                        firstRow.append($("<td>").text(lastRow.Line));
                        firstRow.append($("<td>").text(lastRow.Count));
                        table4.prepend(firstRow);

                        $.each(data.slice(0, data.length - 1), function (index, value) {
                            var row = $("<tr>");
                            row.append($("<td>").text(value.Period));
                            row.append($("<td>").text(value.Line));
                            row.append($("<td>").text(value.Count));
                            table4.append(row);
                        });
                    }
                },
                complete: function () {
                    $("#loading").addClass("hidden");
                }
            });
        }
    </script>
</body>
</html>
