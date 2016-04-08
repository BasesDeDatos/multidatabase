angular.module('manager', [])
    .controller('managerCtrl', [
        '$scope',
        '$http',
        'DBConnections',
        'dataBases',
        'tables',
        'columns',
        function ($scope, $http, DBConnections, dataBases, tables, columns) {

            $scope.DBConnections = false; //se inicia en false para que muestre el loader en la página
            $scope.dataBases = false;
            $scope.tables = tables.all();

            function refreshDBConnections() {
                DBConnections.all().then(function (response) { //Async call to DBConnections factory
                    $scope.DBConnections = response; //Assign data received to $scope.data
                });
            }
            refreshDBConnections();

            function refreshDataBases() {
                dataBases.all().then(function (response) { //Async call to DBConnections factory
                    $scope.dataBases = response; //Assign data received to $scope.data
                });
            }
            refreshDataBases();

            function refreshTables() {
                tables.all().then(function (response) { //Async call to DBConnections factory
                    $scope.tables = response; //Assign data received to $scope.data
                });
            }

            $scope.addDatabase = function () {
                var params = {
                    database_type: $scope.database_type,
                    user: $scope.user,
                    pass: $scope.pass,
                    server: $scope.server,
                    protocol: $scope.protocol,
                    port: $scope.protocol,
                    alias: $scope.alias
                };
                DBConnections.push(params);
                refreshDBConnections();
            }

            $scope.createDatabase = function () {
                var params = { nombre: $scope.DBname };
                dataBases.push(params);
                refreshDatabases();
            }

            $scope.addNewRow = function () {
                new_row = $("tbody tr").first().html();
                $("tbody").append("<tr>"+new_row+"</tr>");
                $("tbody tr").last().find("input, select").val("");
            }
        }
    ]);
