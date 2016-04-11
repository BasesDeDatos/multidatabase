angular.module('manager', [])
    .controller('managerCtrl', [
        '$scope',
        '$http',
        'web_services',
        function ($scope, $http, web_services) {

            $scope.DBConnections = false; //se inicia en false para que muestre el loader en la página
            $scope.databases = false;
            $scope.tables = false;

            web_services.get('getDBConnections').then(function (response) { //Async call to DBConnections factory
                $scope.DBConnections = response; //Assign data received to $scope.data
            });

            web_services.get('getDatabases').then(function (response) { //Async call to DBConnections factory
                $scope.databases = response; //Assign data received to $scope.data
            });

            /*web_services.get('getTables').then(function (response) { //Async call to DBConnections factory
                $scope.tables = response; //Assign data received to $scope.data
            });*/

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
                web_services.post("addDatabase", params, $scope).finally(function () {
                    web_services.get('getDBConnections').then(function (response) { //Async call to DBConnections factory
                        $scope.DBConnections = response; //Assign data received to $scope.data
                    });
                });
            }

            $scope.createDatabase = function () {
                var params = { name: $scope.DB_name };
                web_services.post("createDatabase", params, $scope).finally(function () {
                    web_services.get('getDatabases').then(function (response) { //Async call to DBConnections factory
                        $scope.databases = response; //Assign data received to $scope.data
                    });
                });
                
            }
            
            $scope.createTable = function () {
                var params = {
                    table_name: $scope.table_name,
                    database_id: $scope.database_id,
                    columns: [],
                }
                $("table tbody tr").each(function () {
                    $row = $(this);
                    params.columns.post({
                        DB_alias: $row.find(".table_alias").val(),
                        column_name: $row.find(".table_name").val(),
                        Type: $row.find(".table_type").val(),
                        Null: $row.find(".table_null").val()
                    })
                })
                console.log(params);
                web_services.post("createTable", params, $scope).finally(function () {
                    web_services.get('getTables').then(function (response) { //Async call to DBConnections factory
                        $scope.tables = response; //Assign data received to $scope.data
                    });
                });
                
            }

            $scope.dropTable = function () {
                web_services.delete('dropTables', {}, $scope).finally(function () {
                    web_services.get('getTables').then(function (response) { //Async call to DBConnections factory
                        $scope.tables = response; //Assign data received to $scope.data
                    });
                });
            }

            $scope.addNewRow = function () {
                new_row = $("tbody tr").first().html();
                $("tbody").append("<tr>"+new_row+"</tr>");
                $("tbody tr").last().find("input, select").val("");
            }

            $scope.get_tables = function (ID_database) {
                web_services.get('getTables', { ID: ID_database }).then(function (response) { //Async call to DBConnections factory
                    return response; //Assign data received to $scope.data
                });
            }
        }
    ])