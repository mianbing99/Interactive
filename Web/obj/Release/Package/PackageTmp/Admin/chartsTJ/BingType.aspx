<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BingType.aspx.cs" Inherits="Web.Admin.chartsTJ.BingType" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
       <form id="form1" runat="server">
    <div>
        <div id="container" style="min-width: 310px; height: 400px; margin: 0 auto"></div>
        <div id="container2" style="min-width: 310px; height: 400px; margin: 0 auto"></div>
        <div id="container3" style="min-width: 310px; height: 400px; margin: 0 auto"></div>
    </div>
           </form>
        <script type="text/javascript" src="../js/jquery-1.11.3.js"></script>
    <script type="text/javascript" src="../js/layer/layer.js"></script>
    <script charset="GBK" src="../chartsJs/jquery.js"></script>
    <script charset="GBK" src="../chartsJs/highcharts.js"></script>
    <script charset="GBK" src="../chartsJs/exporting.js" ></script>

<script type="text/javascript">
    $(function () {
        var ytype=['<% =String.Join("','",Ytype) %>'];
        var byslc=parseFloat("<%=byslc%>");
        var bdmec=parseFloat("<%=bdmec%>");
        var bkxtc=parseFloat("<%=bkxtc%>");
        var bjkzc=parseFloat("<%=bjkzc%>");
        var bqwyc=parseFloat("<%=bqwyc%>");
        var byywc=parseFloat("<%=byywc%>");
        var bgxqc=parseFloat("<%=bgxqc%>");
        var bgstc=parseFloat("<%=bgstc%>");
        var bdhpc=parseFloat("<%=bdhpc%>");
        var bwdxc=parseFloat("<%=bwdxc%>");
        var byetc = parseFloat("<%=byetc%>");
        var colors = Highcharts.getOptions().colors,
        categories =ytype,
        data = [{
            y: byslc,
            color: colors[0],
            drilldown: {
                name: ytype[0]+ ' 分类',
                categories: ['<% =String.Join("','",Yyslz) %>'],
                data: [<% =String.Join(",",YZBysl) %>],
                color: colors[0]
            }
        }, {
            y: bdmec,
            color: colors[1],
            drilldown: {
                name: ytype[1] + ' 分类',
                categories: ['<% =String.Join("','",Ydmez) %>'],
                data:  [<% =String.Join(",",YZBdme) %>],
                color: colors[1]
            }
        }, {
            y: bkxtc,
            color: colors[2],
            drilldown: {
                name: ytype[2] + ' 分类',
                categories: ['<% =String.Join("','",Ykxtz) %>'],
                data:  [<% =String.Join(",",YZBkxt)  %>],
                color: colors[2]
            }
        }, {
            y:bjkzc,
            color: colors[3],
            drilldown: {
                name: ytype[3] + ' 分类',
                categories: ['<% =String.Join("','",Yjkzz) %>'],
                data:  [<% =String.Join(",",YZBjkz)  %>],
                color: colors[3]
            }
        }, {
            y: bqwyc,
            color: colors[4],
            drilldown: {
                name: ytype[4] + ' 分类',
                categories: ['<% =String.Join("','",Yqwyz) %>'],
                data:  [<% =String.Join(",",YZBqwy)  %>],
                color: colors[4]
            }
        }, {
            y: byywc,
            color: colors[5],
            drilldown: {
                name: ytype[5] + ' 分类',
                categories: ['<% =String.Join("','",Yyywz) %>'],
                data:  [<% =String.Join(",",YZByyw)  %>],
                color: colors[5]
            }
        }, {
            y: bgxqc,
            color: colors[6],
            drilldown: {
                name: ytype[6] + ' 分类',
                categories: ['<% =String.Join("','",Ygxqz) %>'],
                data:  [<% =String.Join(",",YZBgxq)  %>],
                color: colors[6]
            }
        }, {
            y: bgstc,
            color: colors[7],
            drilldown: {
                name: ytype[7] + ' 分类',
                categories: ['<% =String.Join("','",Ygstz) %>'],
                data:  [<% =String.Join(",",YZBgst)  %>],
                color: colors[7]
            }
        }, {
            y: bdhpc,
            color: colors[8],
            drilldown: {
                name: ytype[8] + ' 分类',
                categories: ['<% =String.Join("','",Ydhpz) %>'],
                data:  [<% =String.Join(",",YZBdhp)  %>],
                color: colors[8]
            }
        }, {
            y: bwdxc,
            color: colors[9],
            drilldown: {
                name: ytype[9] + ' 分类',
                categories: ['<% =String.Join("','",Ywdxz) %>'],
                data:  [<% =String.Join(",",YZBwdx)  %>],
                color: colors[9]
            }
        }, {
            y: byetc,
            color: "#C4541C",
            drilldown: {
                name: ytype[10] + ' 分类',
                categories: ['<% =String.Join("','",Yyetz) %>'],
                data:  [<% =String.Join(",",YZByet) %>],
                color: "#C4541C"
            }
        }
        ],
        browserData = [],
        versionsData = [],
        i,
        j,
        dataLen = data.length,
        drillDataLen,
        brightness;


    // Build the data arrays
    for (i = 0; i < dataLen; i += 1) {

        // add browser data
        browserData.push({
            name: categories[i],
            y: data[i].y,
            color: data[i].color
        });

        // add version data
        drillDataLen = data[i].drilldown.data.length;
        for (j = 0; j < drillDataLen; j += 1) {
            brightness = 0.2 - (j / drillDataLen) / 5;
            versionsData.push({
                name: data[i].drilldown.categories[j],
                y: data[i].drilldown.data[j],
                color: Highcharts.Color(data[i].color).brighten(brightness).get()
            });
        }
    }

    // Create the chart
    Highcharts.chart('container', {
        chart: {
            type: 'pie'
        },
        title: {
            text: 'E家亲:幼儿专区,点击量占比(幼儿伴侣存在的视频,E家亲不存在则不统计)'
        },
        subtitle: {
            text: ''
        },
        yAxis: {
            title: {
                text: ''
            }
        },
        plotOptions: {
            pie: {
                shadow: false,
                center: ['50%', '50%']
            }
        },
        tooltip: {
            valueSuffix: '%'
        },
        series: [{
            name: '占总数%',
            data: browserData,
            size: '60%',
            dataLabels: {
                formatter: function () {
                    return this.y > 5 ? this.point.name : null;
                },
                color: '#ffffff',
                distance: -30
            }
        }, {
            name: '占总数%',
            data: versionsData,
            size: '80%',
            innerSize: '60%',
            dataLabels: {
                formatter: function () {
                    // display only if larger than 1
                    return this.y > 1 ? '<b>' + this.point.name + ':</b> ' + this.y + '%' : null;
                }
            }
        }]
    });
    });
    $(function () {
        var bxqzc=parseFloat("<%=bxqzc%>");
        var bxszc=parseFloat("<%=bxszc%>");
        var bxpzc=parseFloat("<%=bxpzc%>");
        var bgcwc=parseFloat("<%=bgcwc%>");
        var bysjc=parseFloat("<%=bysjc%>");
        var byqc=parseFloat("<%=byqc%>");
        var vtype=['<% =String.Join("','",Otype) %>'];
            var colors = Highcharts.getOptions().colors,
                categories =vtype ,
                data = [{
                    y: bxqzc,
            color: colors[0],
            drilldown: {
                name: vtype[0]+ ' 分类',
                categories: ['<% =String.Join("','",Oxqzz) %>'],
                data: [<% =String.Join(",",OZBxqz) %>],
                color: colors[0]
            }
        }, {
            y: bxszc,
            color: colors[1],
            drilldown: {
                name: vtype[1] + ' 分类',
                categories: ['<% =String.Join("','",Oxszz) %>'],
                data:  [<% =String.Join(",",OZBxsz) %>],
                color: colors[1]
            }
        }, {
            y: bxpzc,
            color: "#C4541C",
            drilldown: {
                name: vtype[2] + ' 分类',
                categories: ['<% =String.Join("','",Oxpzz) %>'],
                data:  [<% =String.Join(",",OZBxpz)  %>],
                color: "#C4541C"
            }
        }, {
            y: bgcwc,
            color: colors[3],
            drilldown: {
                name: vtype[3] + ' 分类',
                categories: ['<% =String.Join("','",Ogcwz) %>'],
                data:  [<% =String.Join(",",OZBgcw)  %>],
                color: colors[7]
            }
        }, {
            y: bysjc,
            color: colors[4],
            drilldown: {
                name: vtype[4] + ' 分类',
                categories: ['<% =String.Join("','",Oysjz) %>'],
                data:  [<% =String.Join(",",OZBysj)  %>],
                color: colors[8]
            }
        }, {
            y: byqc,
            color: colors[5],
            drilldown: {
                name: vtype[5] + ' 分类',
                categories: ['<% =String.Join("','",Oyqz) %>'],
                data:  [<% =String.Join(",",OZByq)  %>],
                color: colors[9]
            }
        }
        ],
        browserData = [],
        versionsData = [],
        i,
        j,
        dataLen = data.length,
        drillDataLen,
        brightness;


            // Build the data arrays
            for (i = 0; i < dataLen; i += 1) {

                // add browser data
                browserData.push({
                    name: categories[i],
                    y: data[i].y,
                    color: data[i].color
                });

                // add version data
                drillDataLen = data[i].drilldown.data.length;
                for (j = 0; j < drillDataLen; j += 1) {
                    brightness = 0.2 - (j / drillDataLen) / 5;
                    versionsData.push({
                        name: data[i].drilldown.categories[j],
                        y: data[i].drilldown.data[j],
                        color: Highcharts.Color(data[i].color).brighten(brightness).get()
                    });
                }
            }

            // Create the chart
            Highcharts.chart('container2', {
                chart: {
                    type: 'pie'
                },
                title: {
                    text: 'E家亲:老人专区,点击量占比'
                },
                subtitle: {
                    text: ''
                },
                yAxis: {
                    title: {
                        text: ''
                    }
                },
                plotOptions: {
                    pie: {
                        shadow: false,
                        center: ['50%', '50%']
                    }
                },
                tooltip: {
                    valueSuffix: '%'
                },
                series: [{
                    name: '占总数%',
                    data: browserData,
                    size: '60%',
                    dataLabels: {
                        formatter: function () {
                            return this.y > 5 ? this.point.name : null;
                        },
                        color: '#ffffff',
                        distance: -30
                    }
                }, {
                    name: '占总数%',
                    data: versionsData,
                    size: '80%',
                    innerSize: '60%',
                    dataLabels: {
                        formatter: function () {
                            // display only if larger than 1
                            return this.y > 1 ? '<b>' + this.point.name + ':</b> ' + this.y + '%' : null;
                        }
                    }
                }]
            });
    });

    $(function () {
        
        $(document).ready(function () {
            var a=parseFloat("<%=OAll%>");
            var b=parseFloat("<%=YAll%>");
            // Build the chart
            Highcharts.chart('container3', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'E家亲点击量占比'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'Brands',
                    colorByPoint: true,
                    data: [{
                        name: '老人专区',
                        y: a
                    }, {
                        name: '幼儿专区',
                        y: b,
                        sliced: true,
                        selected: true
                    }]
                }]
            });
        });
    });
    </script>
</body>
</html>
