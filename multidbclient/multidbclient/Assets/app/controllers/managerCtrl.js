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

            web_services.get('getTables').then(function (response) { //Async call to DBConnections factory
                $scope.tables = response; //Assign data received to $scope.data
            });

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
                web_services.push("addDatabase", params, $scope);
                web_services.get('getDBConnections').then(function (response) { //Async call to DBConnections factory
                    $scope.DBConnections = response; //Assign data received to $scope.data
                });
            }

            $scope.createDatabase = function () {
                var params = { nombre: $scope.DBname };
                web_services.push("createDatabase", params, $scope);
                web_services.get('getDatabases').then(function (response) { //Async call to DBConnections factory
                    $scope.databases = response; //Assign data received to $scope.data
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
                    params.columns.push({
                        DB_alias: $row.find(".table_alias").val(),
                        column_name: $row.find(".table_name").val(),
                        Type: $row.find(".table_type").val(),
                        Null: $row.find(".table_null").val()
                    })
                })
                console.log(params);
                web_services.push("getTables", params, $scope); 
                web_services.get('getTables').then(function (response) { //Async call to DBConnections factory
                    $scope.tables = response; //Assign data received to $scope.data
                });
            }

            $scope.dropTable = function () {
                web_services.delete('dropTables', {}, $scope);
            }

            $scope.addNewRow = function () {
                new_row = $("tbody tr").first().html();
                $("tbody").append("<tr>"+new_row+"</tr>");
                $("tbody tr").last().find("input, select").val("");
            }
        }
    ])

    // I've created this directive as an example of $compile in action. 
    /*.directive('add-column', ['$compile', function ($compile) { // inject $compile service as dependency
        return {
            restrict: 'A',
            link: function ($scope, element, attrs) {
                // click on the button to add new input field
                element.find('button').bind('click', function () {
                    // I'm using Angular syntax. Using jQuery will have the same effect
                    // Create input element
                    var input = angular.element('<div><input type="text" ng-model="telephone[' + scope.inputCounter + ']"></div>');
                    // Compile the HTML and assign to scope
                    var compile = $compile(input)(scope);

                    // Append input to div
                   element.append(input);

                    // Increment the counter for the next input to be added
                    scope.inputCounter++;
                });
            }
        }
    }]);*/
