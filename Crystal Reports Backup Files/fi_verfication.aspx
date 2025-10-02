<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fi_verfication.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="bbq.fi_verfication" EnableEventValidation="false" %>

<!DOCTYPE html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>ERP</title>

    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@300;400;600;700;800&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="../assets/css/bootstrap.css">

    <link rel="stylesheet" href="../assets/vendors/iconly/bold.css">

    <link rel="stylesheet" href="../assets/vendors/perfect-scrollbar/perfect-scrollbar.css">
    <link rel="stylesheet" href="../assets/vendors/bootstrap-icons/bootstrap-icons.css">
    <link rel="stylesheet" href="../assets/css/app.css">
    <link rel="shortcut icon" href="../assets/images/favicon.svg" type="image/x-icon">


    <script type="text/javascript">



        function yesnoCheck() {


            if (document.getElementById('radio_cheque').checked) {
                document.getElementById('bt_cheque_refrence').style.visibility = 'visible';
            } else {
                document.getElementById('bt_cheque_refrence').style.visibility = 'hidden';

            }
        }

    </script>



</head>

<body>
    <form id="Form1" runat="server">
        <div id="app">
            <div id="sidebar" class="active">
                <div class="sidebar-wrapper active">
                    <div class="sidebar-header">
                        <div class="d-flex justify-content-between">
                            <div class="logo">
                                <a href="index.html">
                                    <img src="../assets/images/logo/logo.png" alt="Logo" srcset=""></a>
                            </div>
                            <div class="toggler">
                                <a href="#" class="sidebar-hide d-xl-none d-block"><i class="bi bi-x bi-middle"></i></a>
                            </div>
                        </div>
                    </div>
                     <div class="sidebar-menu">
                        <ul class="menu">
                            <li class="sidebar-title">Home</li>


                            <li class="sidebar-item has-sub" id="li_dashboard" runat="server">
                                <a href="#" class='sidebar-link'>
                                    <i class="bi bi-stack"></i>
                                    <span>Dashboard</span>
                                </a>
                                <ul class="submenu active">
                                    <li class="submenu-item active ">
                                        <a href="dashboard.aspx">Finance</a>
                                    </li>
                                </ul>
                            </li>



                            <li class="sidebar-title" id="menu_finance_head" runat="server">Finance</li>

                            <li class="sidebar-item  has-sub" id="menu_data_entry" runat="server">
                                <a href="#" class='sidebar-link'>
                                    <i class="bi bi-hexagon-fill"></i>
                                    <span>Data Entry</span>
                                </a>
                                <ul class="submenu">
                                    <li class="submenu-item ">
                                        <a href="single_entry.aspx">Single Entry</a>
                                    </li>
                                    <li class="submenu-item ">
                                        <a href="double_entry.aspx">Double Entry</a>
                                    </li>

                                </ul>
                            </li>



                            <li
                                class="sidebar-item   has-sub" id="menu_settings" runat="server">
                                <a href="#" class='sidebar-link '>
                                    <i class="bi bi-pen-fill"></i>
                                    <span>Settings</span>
                                </a>
                                <ul class="submenu ">
                                    <li class="submenu-item">
                                        <a href="register.aspx">Profile</a>
                                    </li>
                                    <li class="submenu-item ">
                                        <a href="account_create.aspx">Account Create</a>
                                    </li>
                                    <li class="submenu-item">
                                        <a href="account_category_create.aspx">Account Category Create</a>
                                    </li>
                                </ul>
                            </li>



                            <li
                                class="sidebar-item  has-sub" id="menu_reports" runat="server">
                                <a href="#" class='sidebar-link'>
                                    <i class="bi bi-file-earmark-spreadsheet-fill"></i>
                                    <span>Reports</span>
                                </a>
                                <ul class="submenu ">
                                    <li class="submenu-item ">
                                        <a href="fi_statements.aspx">Statments</a>
                                    </li>
                                    <li class="submenu-item ">
                                        <a href="fi_summary.aspx">Account Summary</a>
                                    </li>
                                    <li class="submenu-item ">
                                        <a href="fi_finle_reports.aspx">Finle Reports</a>
                                    </li>
                                </ul>
                            </li>

                            <li class="sidebar-title" id="menu_administartion_head" runat="server">Administration</li>

                            <li
                                class="sidebar-item  has-sub" id="menu_authorization" runat="server">
                                <a href="#" class='sidebar-link'>
                                    <i class="bi bi-pentagon-fill"></i>
                                    <span>Authorization</span>
                                </a>
                                <ul class="submenu ">
                                    <li class="submenu-item ">
                                        <a href="fi_authorization.aspx">Finance</a>
                                    </li>
                                </ul>
                            </li>
                            <li
                                class="sidebar-item active has-sub" id="menu_verification" runat="server">
                                <a href="#" class='sidebar-link'>
                                    <i class="bi bi-pentagon-fill"></i>
                                    <span>Verification</span>
                                </a>
                                <ul class="submenu active">
                                    <li class="submenu-item active">
                                        <a href="fi_verfication.aspx">Finance</a>
                                    </li>

                                </ul>
                            </li>

                            <li class="sidebar-title"><a href="default.aspx">Sign Out</a></li>
                        </ul>
                    </div>
                    <button class="sidebar-toggler btn x"><i data-feather="x"></i></button>
                </div>
            </div>
            <div id="main">
                <header class="mb-3">
                    <a href="#" class="burger-btn d-block d-xl-none">
                        <i class="bi bi-justify fs-3"></i>
                    </a>
                </header>


                <div class="page-content">

                    <section class="row">
                        <div class="col-12 ">
                            <div class="row">
                                <div class="card">

                                    <div class="card-body">

                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Account Type</label>
                                                    <div class="form-group">
                                                        <asp:DropDownList CssClass="form-control" ID="category_type" runat="server" OnSelectedIndexChanged="account_type_SelectedIndexChanged" AutoPostBack="True" BorderColor="#3399ff">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>
                            <div class="col-12 ">
                              <div class="row" >
                                <div class="card">

                                    <div class="card-body">

                                        <asp:GridView ID="table1" runat="server" AutoGenerateColumns="true"
                                            CssClass="table table-striped" OnRowDeleting="GridView1_RowDeleting" OnRowCommand="table1_RowCommand">

                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="3px">

                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton2" Text="Verify" runat="server" CommandName="verify" CommandArgument="<%# Container.DataItemIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                                </div>
                        </div>
                        
                    </section>
                    <section class="row">
                        <div class="col-9 ">
                            
                          
                        </div>

                    </section>
                </div>

                <footer>
                    <div class="footer clearfix mb-0 text-muted">
                        <div class="float-start">
                            <p>2022 &copy; Blind Cat Solution</p>
                        </div>

                    </div>
                </footer>
            </div>
        </div>

    </form>
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
        var table1 = document.querySelector('#table1');
        var dataTable = new simpleDatatables.DataTable(table1);
</script>





</body>

