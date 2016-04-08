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
            
            $scope.addTable = function () {
                var params = {
                    name: $scope.nombre_table,
                    columns: {}
                }
                $("table tr").each(function(){
                    $row = $(this);
                    params.columns.push({
                        DB_alias: $row.find(".table_type").val(),
                        column_name: column_name,
                        Type: Type,
                        Null: Null
                    })
                })
                
            }

            $scope.addNewRow = function () {
                new_row = $("tbody tr").first().html();
                $("tbody").append("<tr>"+new_row+"</tr>");
                $("tbody tr").last().find("input, select").val("");
            }
        }
    ])
    // I've created this directive as an example of $compile in action. 
    .directive('add-column', ['$compile', function ($compile) { // inject $compile service as dependency
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
}]);
