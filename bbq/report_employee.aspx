<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report_employee.aspx.cs" Inherits="bbq.report_employee" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <title>HRIS</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="icon" href="favicon.ico" type="image/x-icon" />
    <!-- END META SECTION -->
    <link href="jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="jquery.min.js" type="text/javascript"></script>
    <script src="jquery-ui.min.js" type="text/javascript"></script>
    <!-- CSS INCLUDE -->
    <link rel="stylesheet" type="text/css" id="theme" href="css/theme-default.css" />
    <!-- EOF CSS INCLUDE -->

    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@300;400;600;700;800&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="../assets/css/bootstrap.css">

    <link rel="stylesheet" href="../assets/vendors/iconly/bold.css">

    <link rel="stylesheet" href="../assets/vendors/perfect-scrollbar/perfect-scrollbar.css">
    <link rel="stylesheet" href="../assets/vendors/bootstrap-icons/bootstrap-icons.css">
    <link rel="stylesheet" href="../assets/css/app.css">
    <link rel="shortcut icon" href="../assets/images/favicon.svg" type="image/x-icon">
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
                            <img src="assets/images/users/avatar.jpg" alt="John Doe" />
                        </a>
                        <div class="profile">
                            <div class="profile-image">
                                <img src="assets/images/users/avatar.jpg" alt="John Doe" />
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
                    <li class="xn-openable ">
                        <a href="#"><span class="fa fa-cogs"></span><span class="xn-text">On-Going Function</span></a>
                        <ul>
                            <li><a href="deduction.aspx"><span class="fa fa-heart"></span>Deductions & Allowances</a></li>
                            <li><a href="leave.aspx"><span class="fa fa-square-o"></span>Leave</a></li>

                        </ul>
                    </li>
                    <li class="xn-openable active">
                        <a href="#"><span class="fa fa-pencil"></span><span class="xn-text">Reports</span></a>
                        <ul>
                            <li><a href="report_salary_summary.aspx"><span class="fa fa-file-text-o"></span>Salary Summary (Employee)</a></li>
                            <li><a href="report_employee.aspx"><span class="fa fa-file-text-o"></span>Employee Report</a></li>
                             <li class="active"><a href="advance_reports.aspx"><span class="fa fa-file-text-o"></span>Other Reports</a></li>

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
                    <li class="active">Reports</li>
                     <li class="active">Employee Report</li>
                </ul>
                <!-- END BREADCRUMB -->

                <!-- PAGE CONTENT WRAPPER -->
                <div class="page-content-wrap">




                    <div class="row">
                        <div class="col-md-12">

                            <!-- START DATATABLE EXPORT -->
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Employee Report</h3>
                                    <div class="btn-group pull-right">
                                        <button class="btn btn-danger dropdown-toggle" data-toggle="dropdown"><i class="fa fa-bars"></i>Export Data</button>
                                        <ul class="dropdown-menu">
                                           
                                            <li><a href="#" onclick="$('#Grid_employee').tableExport({type:'excel',escape:'false'});">
                                                <img src='img/icons/xls.png' width="24" />
                                                XLS</a></li>
                                          
                                        </ul>
                                    </div>

                                </div>
                                <div class="panel-body">
                                    <asp:GridView ID="Grid_employee" runat="server" AutoGenerateColumns="true" AutoPostback="Flash"
                                        CssClass="table table-striped">
                                    </asp:GridView>
                                </div>
                            </div>
                            <!-- END DATATABLE EXPORT -->



                        </div>
                    </div>
                    <div class="row">
                        <p style="color: orange">Advance Search Customization(Export to Excel Only)</p>
                        <div class="col-md-12">

                            <panel class="form-horizontal">

                                <div class="panel panel-default tabs">
                                    <ul class="nav nav-tabs" role="tablist">
                                        <li class="active"><a href="#tab-first" role="tab" data-toggle="tab">Column Selection</a></li>
                                        <li><a href="#tab-second" role="tab" data-toggle="tab">Order By Selection </a></li>
                                    </ul>
                                    <div class="panel-body tab-content">
                                        <div class="tab-pane active" id="tab-first">
                                            <p style="color: #ff6a00">This Section Enable you to Select which Column's need to be Enable in Report</p>

                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_epfPay_" Text="EPF Pay" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_epfNo" Text="EPF No" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="CheckBox1" Text="Attendance No" runat="server" />
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                    <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_name" Text="Full Name" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_nic" Text="NIC" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_drivingLicence" Text="Driving Licence" runat="server" />
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                    <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_dob" Text="DOB" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_gender" Text="Gender" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_marital" Text="Marital Status" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_blood" Text="Blood Group" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_religion" Text="Religion" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_race" Text="Race" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_nationality" Text="Nationality" runat="server" />
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                    <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_department" Text="Department" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_shiftBlock" Text="ShiftBlock" runat="server" />
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                    <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_address" Text="Address" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_contact" Text="Conatct" runat="server" />
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                    <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_basic" Text="Basic" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_budj" Text="Budgetary" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_attendance" Text="Attendance" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_allow" Text="Allowances" runat="server" />
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                    <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_bankPay" Text="Bank Pay" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_acNo" Text="Account No" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_bankCode" Text="Bank Code" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_branchCode" Text="Branch Code" runat="server" />
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                    <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_resign" Text="Resgin" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_resginDate" Text="Resgin Date" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_doa" Text="Appointment Date" runat="server" />
                                                        </div>
                                                        <div class="col-md-1 col-xs-12">
                                                            <asp:CheckBox ID="check_block" Text="Block" runat="server" />
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                    <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_defaultRoaster" Text="Default Roaster" runat="server" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:CheckBox ID="check_otTYpe" Text="OT Circle Mode" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="panel-footer">
                                                <asp:Button ID="btn_saveAdvanceSearch" class="btn btn-primary pull-right" Text="Save Changes" runat="server" OnClick="btn_saveAdvanceSearch_Click" />
                                            </div>

                                        </div>
                                        <div class="tab-pane" id="tab-second">
                                            <p style="color: #ff6a00">This Section Enable you to Select Order By Clause</p>
                                            <div class="row">
                                                <div class="col-md-6">
                                                     <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="check_orderByEpfNo" Text="Order By EPF No" runat="server" GroupName="A"/>
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="radio_orderByEpfNoASC" Text="ASC" runat="server" GroupName="1" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="radio_orderByEpfNoDESC" Text="DESC" runat="server" GroupName="1" />
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                    <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="check_orderByAttendanceId" Text="Order By Attendance No" runat="server" GroupName="A" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="radio_orderByAttendanceIdASC" Text="ASC" runat="server" GroupName="2" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="radio_orderByAttendanceIdDESC" Text="DESC" runat="server" GroupName="2"/>
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                     <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="check_orderBySystemID" Text="Order By System Entered" runat="server"  GroupName="A"/>
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="radio_orderBySystemIDASC" Text="ASC" runat="server" GroupName="3"/>
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="radio_orderBySystemIDDESC" Text="DESC" runat="server" GroupName="3" />
                                                        </div>
                                                    </div>
                                                    <hr width="100%" color="red" align="center">
                                                     <div class="form-group">
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="check_orderByName" Text="Order By Name" runat="server"  GroupName="A"/>
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="radio_orderByNameASC" Text="ASC" runat="server" GroupName="4" />
                                                        </div>
                                                        <div class="col-md-2 col-xs-12">
                                                            <asp:RadioButton ID="radio_orderByNameDESC" Text="DESC" runat="server" GroupName="4" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="panel-footer">
                                                <asp:Button ID="Button1" class="btn btn-primary pull-right" Text="Save Changes" runat="server" OnClick="btn_saveAdvanceSearch_Click" />
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
        <script src="../assets/vendors/perfect-scrollbar/perfect-scrollbar.min.js"></script>
        <script src="../assets/js/bootstrap.bundle.min.js"></script>

        <script src="../assets/vendors/apexcharts/apexcharts.js"></script>
        <script src="../assets/js/pages/dashboard.js"></script>
        <script src="../assets/js/mazer.js"></script>


        <script src="../assets/vendors/perfect-scrollbar/perfect-scrollbar.min.js"></script>
        <script src="../assets/js/bootstrap.bundle.min.js"></script>
        <script src="../assets/vendors/simple-datatables/simple-datatables.js"></script>
        <script>
            // Simple Datatable
            var table1 = document.querySelector('#Grid_employee');
            var dataTable = new simpleDatatables.DataTable(table1);
        </script>
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

        <script type='text/javascript' src='js/plugins/icheck/icheck.min.js'></script>
        <script type="text/javascript" src="js/plugins/mcustomscrollbar/jquery.mCustomScrollbar.min.js"></script>

        <script type="text/javascript" src="js/plugins/datatables/jquery.dataTables.min.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/tableExport.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/jquery.base64.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/html2canvas.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/jspdf/libs/sprintf.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/jspdf/jspdf.js"></script>
        <script type="text/javascript" src="js/plugins/tableexport/jspdf/libs/base64.js"></script>
        <!-- END THIS PAGE PLUGINS-->
        <!-- START TEMPLATE -->
        <script type="text/javascript" src="js/settings.js"></script>

        <script type="text/javascript" src="js/plugins.js"></script>
        <script type="text/javascript" src="js/actions.js"></script>
    </form>
</body>
</html>
