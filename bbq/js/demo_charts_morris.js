var morrisCharts = function() {

    Morris.Line({
      element: 'morris-line-example',
      data: [
          { y: '2023-01-17', a: 100, b: 90, C:15,d:80 ,e:46},
          { y: '2023-01-18', a: 75, b: 65, C: 60, d: 72, e: 5 },
          { y: '2023-01-19', a: 50, b: 40, C: 21, d: 49, e: 33},
          { y: '2023-01-20', a: 75, b: 65, C: 56, d: 8, e: 35},
          { y: '2023-01-21', a: 50, b: 40, C: 14, d: 12, e: 50},
          { y: '2023-01-22', a: 75, b: 65, C: 54, d: 54, e: 15},
          { y: '2023-01-23', a: 100, b: 90, C: 32, d: 23, e: 96 }
      ],
      xkey: 'y',
      ykeys: ['a', 'b','C','d','e'],
        labels: ['Productin', 'HR', 'Cleaning','Finance','Reception'],
      resize: true,
        lineColors: ['#33414E', '#95B75D', '#FEA223', '#B64645', '#1caf9a']
    });


    Morris.Area({
        element: 'morris-area-example',
        data: [
            { y: '2006', a: 100, b: 90 },
            { y: '2007', a: 75,  b: 65 },
            { y: '2008', a: 50,  b: 40 },
            { y: '2009', a: 75,  b: 65 },
            { y: '2010', a: 50,  b: 40 },
            { y: '2011', a: 75,  b: 65 },
            { y: '2012', a: 100, b: 90 }
        ],
        xkey: 'y',
        ykeys: ['a', 'b'],
        labels: ['Series A', 'Series B'],
        
        resize: true,
        lineColors: ['#1caf9a', '#FEA223']
    });


    Morris.Bar({
        element: 'morris-bar-example',
        data: [
            { y: '2006', a: 100, b: 90 },
            { y: '2007', a: 75,  b: 65 },
            { y: '2008', a: 50,  b: 40 },
            { y: '2009', a: 75,  b: 65 },
            { y: '2010', a: 50,  b: 40 },
            { y: '2011', a: 75,  b: 65 },
            { y: '2012', a: 100, b: 90 }
        ],
        xkey: 'y',
        ykeys: ['a', 'b'],
        labels: ['Series A', 'Series B'],
        barColors: ['#B64645', '#33414E']
    });


    Morris.Donut({
        element: 'morris-donut-example',
        data: [
            {label: "Download Sales", value: 12},
            {label: "In-Store Sales", value: 30},
            {label: "Mail-Order Sales", value: 20}
        ],
        colors: ['#95B75D', '#1caf9a', '#FEA223']
    });

}();