<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActiveTime.aspx.cs" Inherits="Web.Admin.chartsTJ.ActiveTime" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container" style="min-width: 310px; height: 400px; margin: 0 auto"></div>

    </form>
    <script charset="GBK" src="../chartsJs/highcharts.js"></script>
    <script charset="GBK" src="../chartsJs/exporting.js"></script>
    <script>
        var data = [<%=String.Join(",",time)%>];
        var count = '<%=c%>';
        var oDate = new Date(); //实例一个时间对象；
        oDate.setTime(oDate.getTime() - 24 * 60 * 60 * 1000);
        var y = oDate.getFullYear();   //获取系统的年；
        var m = oDate.getMonth();   //获取系统月份，由于月份是从0开始计算，所以要加1
        var d = oDate.getDate(); // 获取系统日，

        Highcharts.chart('container', {
            chart: {
                type: 'spline'
            },
            title: {
                text: '昨日点击量共' + count + '次'
            },
            subtitle: {
                text: '可以看出不同时间的点击量大小'
            },
            xAxis: {
                type: 'datetime',
                labels: {
                    overflow: 'justify'
                }
            },
            yAxis: {
                title: {
                    text: '点击次数'
                },
                minorGridLineWidth: 0,
                gridLineWidth: 0,
                alternateGridColor: null,
                plotBands: []
            },
            tooltip: {
                valueSuffix: ' 次'
            },
            plotOptions: {
                spline: {
                    lineWidth: 4,
                    states: {
                        hover: {
                            lineWidth: 5
                        }
                    },
                    marker: {
                        enabled: false
                    },
                    pointInterval: 3600000, // one hour
                    pointStart: Date.UTC(y, m, d, 0, 0, 0)
                }
            },
            series: [{
                name: '点击量',
                data: data

            }],
            navigation: {
                menuItemStyle: {
                    fontSize: '10px'
                }
            }
        });
    </script>
</body>
</html>
