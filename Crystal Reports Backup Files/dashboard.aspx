<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="bbq.dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>ERP</title>

    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@300;400;600;700;800&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="assets/css/bootstrap.css">

    <link rel="stylesheet" href="assets/vendors/iconly/bold.css">

    <link rel="stylesheet" href="assets/vendors/perfect-scrollbar/perfect-scrollbar.css">
    <link rel="stylesheet" href="assets/vendors/bootstrap-icons/bootstrap-icons.css">
    <link rel="stylesheet" href="assets/css/app.css">
    <link rel="shortcut icon" href="assets/images/favicon.svg" type="image/x-icon">

    <script type="text/ecmascript">
        function myFunction() {
            document.getElementById("dashboard").disabled = true;
        }
    </script>
    <style type="text/css">
        #chart {
            max-width: 750px;
            margin: 35px auto;
        }
    </style>
</head>

<body>
    <form id="Form1" runat="server">
        <div id="app">
            <div id="sidebar" class="active">
                <div class="sidebar-wrapper active">
                    <div class="sidebar-header">
                        <div class="d-flex justify-content-between">
                            <div class="logo">
                                <a href="default.aspx">
                                    <img src="assets/images/logo/logo.png" alt="Logo" srcset=""></a>
                            </div>
                            <div class="toggler">
                                <a href="#" class="sidebar-hide d-xl-none d-block"><i class="bi bi-x bi-middle"></i></a>
                            </div>
                        </div>
                    </div>
                    <div class="sidebar-menu">
                        <ul class="menu">
                            <li class="sidebar-title">Home</li>


                            <li class="sidebar-item active has-sub" id="li_dashboard" runat="server">
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



                           
                            <li class="sidebar-item  has-sub" id="menu_data_entry" runat="server">
                                <a href="#" class='sidebar-link'>
                                    <i class="bi bi-hexagon-fill"></i>
                                    <span>Summary</span>
                                </a>
                                <ul class="submenu">
                                    <li class="submenu-item ">
                                        <a href="daybook.aspx">Daybook</a>
                                    </li>
                                     <li class="submenu-item">
                                        <a href="customer.aspx">Customer</a>
                                    </li>
                                    <li class="submenu-item">
                                        <a href="sale_history.aspx">Outstanding ( Rent )</a>
                                    </li>
                                     <li class="submenu-item">
                                        <a href="stock.aspx">Stock</a>
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
                        <div class="col-12 col-lg-9">
                            <div class="row">
                                <div class="col-6 col-lg-5 col-md-6">
                                    <div class="card">
                                        <div class="card-body px-3 py-4-5">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="stats-icon blue">
                                                        <i class="iconly-boldArrow---Left-Circle"></i>
                                                    </div>
                                                </div>
                                                <div class="col-md-8">
                                                    <h6 class="text-muted font-semibold"><a href="daybook.aspx?branch=hardware">Hardware</a></h6>
                                                    <h6 class="font-extrabold mb-0" id="hardware_cash" runat="server">0.00</h6>
                                                    <p class="font-extrabold mb-0" id="hardware_sale" runat="server" style="color: #ff6a00; font-size: smaller">></p>
                                                    <p class="font-extrabold mb-0" id="hardware_time" runat="server" style="color: #808080; font-size: xx-small">></p>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6 col-lg-5 col-md-6">
                                    <div class="card">
                                        <div class="card-body px-3 py-4-5">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="stats-icon green">
                                                        <i class="iconly-boldArrow---Right-Circle"></i>
                                                    </div>
                                                </div>
                                                <div class="col-md-8">
                                                    <h6 class="text-muted font-semibold"><a href="daybook.aspx?branch=moratuwa">Moratuwa</a></h6>
                                                    <h6 class="font-extrabold mb-0" id="moratuwa_cash" runat="server">0.00</h6>
                                                    <p class="font-extrabold mb-0" id="moratuwa_sale" runat="server" style="color: #ff6a00; font-size: smaller">></p>
                                                    <p class="font-extrabold mb-0" id="moratuwa_time" runat="server" style="color: #808080; font-size: xx-small">></p>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-6 col-lg-5 col-md-6">
                                    <div class="card">
                                        <div class="card-body px-3 py-4-5">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="stats-icon green">
                                                        <i class="iconly-boldArrow---Down-Circle"></i>
                                                    </div>
                                                </div>
                                                <div class="col-md-8">
                                                    <h6 class="text-muted font-semibold"><a href="daybook.aspx?branch=panadura">Pandura</a></h6>
                                                    <h6 class="font-extrabold mb-0" id="panadura_cash" runat="server">0.00</h6>
                                                    <p class="font-extrabold mb-0" id="panadura_sale" runat="server" style="color: #ff6a00; font-size: smaller">></p>
                                                    <p class="font-extrabold mb-0" id="panadura_time" runat="server" style="color: #808080; font-size: xx-small">></p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6 col-lg-5 col-md-6">
                                    <div class="card">
                                        <div class="card-body px-3 py-4-5">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="stats-icon red">
                                                        <i class="iconly-boldArrow---Up-Circle"></i>
                                                    </div>
                                                </div>
                                                <div class="col-md-8">
                                                    <h6 class="text-muted font-semibold"><a href="daybook.aspx?branch=rawatawatta">Rawatawatta</a></h6>
                                                    <h6 class="font-extrabold mb-0" id="rawatawatta_cash" runat="server">0.00</h6>
                                                    <p class="font-extrabold mb-0" id="rawatawatta_sale" runat="server" style="color: #ff6a00; font-size: smaller">></p>
                                                    <p class="font-extrabold mb-0" id="rawatawatta_time" runat="server" style="color: #808080; font-size: xx-small">></p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6 col-lg-5 col-md-6">
                                    <div class="card">
                                        <div class="card-body px-3 py-4-5">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="stats-icon red">
                                                        <i class="iconly-boldArrow---Up-Circle"></i>
                                                    </div>
                                                </div>
                                                <div class="col-md-8">
                                                    <h6 class="text-muted font-semibold"><a href="daybook.aspx?branch=pamankada">Pamankada</a></h6>
                                                    <h6 class="font-extrabold mb-0" id="pamankada_cash" runat="server">0.00</h6>
                                                    <p class="font-extrabold mb-0" id="pamankada_sale" runat="server" style="color: #ff6a00; font-size: smaller">></p>
                                                    <p class="font-extrabold mb-0" id="pamankaa_time" runat="server" style="color: #808080; font-size: xx-small">></p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                

                            </div>
                          
                           
                        </div>
                        <div class="col-12 col-lg-3">

                            <div class="card">

                                <div class="card-content pb-4">
                                    <div class="recent-message d-flex px-4 py-3">
                                        <div class="avatar avatar-lg">
                                            <img src="assets/images/faces/4.jpg">
                                        </div>
                                        <div class="name ms-4">
                                            <h6 class="mb-1">Total Cash</h6>
                                            <p class="text-muted mb-0" id="total_cash" runat="server" style="color:#ff6a00; font-size: large"></p>
                                        </div>
                                    </div>
                                    <div class="recent-message d-flex px-4 py-3">
                                        <div class="avatar avatar-lg">
                                            <img src="assets/images/faces/5.jpg">
                                        </div>
                                        <div class="name ms-4">
                                            <h6 class="mb-1">Total Direct Sale</h6>
                                            <p class="text-muted mb-0" id="total_direct_sale" runat="server" style="color: #00ff21; font-size: large"></p>
                                        </div>
                                    </div>
                                     <div class="recent-message d-flex px-4 py-3">
                                        <div class="avatar avatar-lg">
                                            <img src="assets/images/faces/5.jpg">
                                        </div>
                                        <div class="name ms-4">
                                            <h6 class="mb-1">Total Rent Sale</h6>
                                            <p class="text-muted mb-0" id="total_rent_sale" runat="server" style="color: #00ff21; font-size: large"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </section>

                </div>

                <footer>
                    <div class="footer clearfix mb-0 text-muted">
                        <div class="float-start">
                            <p>2022 &copy; Java Solution (Pvt) Ltd</p>
                        </div>
                        <div class="float-end">
                            <p>
                            </p>
                        </div>
                    </div>
                </footer>
            </div>
        </div>

    </form>
    <script src="assets/vendors/perfect-scrollbar/perfect-scrollbar.min.js"></script>
    <script src="assets/js/bootstrap.bundle.min.js"></script>

    <script src="assets/vendors/apexcharts/apexcharts.js"></script>
    <script src="assets/js/pages/dashboard.js"></script>
    <script src="assets/js/mazer.js"></script>


    <script src="assets/vendors/perfect-scrollbar/perfect-scrollbar.min.js"></script>
    <script src="assets/js/bootstrap.bundle.min.js"></script>

    <script src="assets/vendors/simple-datatables/simple-datatables.js"></script>
    <script type="text/javascript">




</script>





</body>

</html>


