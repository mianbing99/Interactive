﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Line2.aspx.cs" Inherits="Web.Admin.chartsTJ.Line2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
       <form id="form1" runat="server">
    <div>
      <div id="container2" style="min-width: 310px; height: 400px; margin: 0 auto"></div>
    </div>
    </form>
    <script type="text/javascript" src="../js/jquery-1.11.3.js"></script>
    <script type="text/javascript" src="../js/layer/layer.js"></script>
    <script type="text/javascript" src="../chartsJs/jquery.js"></script>
    <script type="text/javascript" src="../chartsJs/highcharts.js"></script>
    <script type="text/javascript" src="../chartsJs/exporting.js"></script>

<script type="text/javascript">

    $(function () {
        var dataa = new Array();
        var zdataa = new Array();
        var y = 2016, m = 12, d = 5;//起始日期
        m = m - 1;
        var data2_2 = [<% =String.Join(",", time2) %>];
        for (var i = 0; i < data2_2.length - 1; i++) {
            var zdataa = new Array();
            zdataa[0] = Date.UTC(y, m, d);
            zdataa[1] = data2_2[i];
            dataa[i] = zdataa;
            if (m != 1) {
                if (m != 4 && m != 6 && m != 9 && m != 11)//大月31天
                {
                    if (d >= 31) {
                        d = 1; m++;
                        if (m > 12) {
                            y++; m = 1; d = 1;
                        }
                    } else {
                        d++;
                    }

                } else {//小月30天
                    if (d > 30) {
                        d = 1; m++;
                        if (m > 12) {
                            y++; m = 1; d = 1;
                        }
                    } else
                        d++;
                }
            } else {//2月
                if ((y % 4 == 0 && y % 100 != 0) || (y % 400 == 0)) {//29天
                    if (d > 29) {
                        d = 1; m++;
                        if (m > 12) {
                            y++; m = 1; d = 1;
                        }
                    } else
                        d++;
                } else {//28天
                    if (d > 28) {
                        d = 1; m++;
                        if (m > 12) {
                            y++; m = 1; d = 1;
                        }
                    } else
                        d++;
                }
            }

        }
        Highcharts.chart('container2', {
            chart: {
                zoomType: 'x'
            },
            title: {
                text: 'IP总数走势图(不包含重复)'
            },
            subtitle: {
                text: document.ontouchstart === undefined ?
                        '' : ''
            },
            xAxis: {
                type: 'datetime'
            },
            yAxis: {
                title: {
                    text: 'IP总数'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                area: {
                    fillColor: {
                        linearGradient: {
                            x1: 0,
                            y1: 0,
                            x2: 0,
                            y2: 1
                        },
                        stops: [
                            [0, Highcharts.getOptions().colors[0]],
                            [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                        ]
                    },
                    marker: {
                        radius: 2
                    },
                    lineWidth: 1,
                    states: {
                        hover: {
                            lineWidth: 1
                        }
                    },
                    threshold: null
                }
            },

            series: [{
                type: 'area',
                name: 'IP总数',
                data: dataa
            }]
        });

    });

</script>
</body>
</html>