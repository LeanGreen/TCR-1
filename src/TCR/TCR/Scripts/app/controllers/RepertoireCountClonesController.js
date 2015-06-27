﻿tcrApp.controller("RepertoireCountClonesController",
    function ($scope, $http) {
        $scope.loaded = false;
        $scope.chartConfig = {
            loading: true,
            title: {
                text: 'Количество общих клонотипов'
            }
        };
        $scope.init = function () {
            $http.get('../api/Service/GetCountClones').success(function (response) {
                $scope.chartConfig = {
                    options: {
                        chart: {
                            type: 'heatmap'
                        },
                        colorAxis: {
                            min: 0,
                            minColor: '#FFFFFF',
                            maxColor: Highcharts.getOptions().colors[0]
                        },
                        tooltip: {
                            formatter: function () {
                                return '<b>' + this.series.xAxis.categories[this.point.x] + '</b> имеет <br><b>' +
                                    this.point.value + '</b> общих клонотипов с <br><b>' + this.series.yAxis.categories[this.point.y] + '</b>';
                            }
                        },
                        xAxis: {
                            categories: response.Repertoires
                        },
                        yAxis: {
                            categories: response.Repertoires,
                            title: null
                        },
                        legend: {
                            align: 'right',
                            layout: 'vertical',
                            margin: 0,
                            verticalAlign: 'top',
                            y: 25,
                            symbolHeight: 280
                        }
                    },

                    title: {
                        text: 'Количество общих клонотипов'
                    },

                    series: [{
                        name: 'Количество общих клонотипов',
                        borderWidth: 1,
                        data: response.Data,
                        dataLabels: {
                            enabled: true,
                            color: '#000000'
                        }
                    }],

                    loading: false
                }
            });
        }
        $scope.init();
    }
);