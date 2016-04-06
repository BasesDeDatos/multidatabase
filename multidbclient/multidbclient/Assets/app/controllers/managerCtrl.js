angular.module('manager', [])
    .controller('managerCtrl', [
        '$scope',
        '$http',
        'DBConnections',
        'dataBases',
        'tables',
        'columns',
        function ($scope, $http, DBConnections, dataBases, tables, columns) {

            DBConnections.all().then(function(response){ 
                $scope.DBConnections = response; //Assign data received to $scope.data
                console.log("DEBUG FACTORY DBConnections then");
                console.log($scope.DBConnections);
            });

            console.log("DEBUG FACTORY DBConnections");
            console.log($scope.DBConnections);

            $scope.DBConnections = DBConnections;
            $scope.dataBases = dataBases.all();
            $scope.tables = tables.all();

            $scope.addDatabase = function () {
                $scope.showMessage = false;

                var params = {
                    database_type: $scope.database_type,
                    user: $scope.user,
                    pass: $scope.pass,
                    server: $scope.server,
                    protocol: $scope.protocol,
                    port: $scope.protocol,
                    alias: $scope.alias
                };
                $http({
                    url: 'http://localhost:8080/service/multiDBService.svc/addDatabase',
                    method: "POST",
                    headers: { 'Content-Type': 'application/json' },
                    data: params
                })
                .success(function (data, status, headers, config) {
                    console.log(JSON.stringify(data));
                    alert("revise la consola");
                })
                .error(function (data, status, headers, config) {
                    $scope.message = data.error_description.replace(/["']{1}/gi, "");
                    $scope.showMessage = true;
                    console.log(JSON.stringify(data));
                    alert("revise la consola");
                });
            }

            $scope.addNewRow = function () {
                new_row = $("tbody tr").first().html();
                $("tbody").append("<tr>"+new_row+"</tr>");
                $("tbody tr").last().find("input, select").val("");
            }
        }
    ]);