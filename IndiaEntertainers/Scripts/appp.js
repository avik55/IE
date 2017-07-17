(function () {
    var secretEmptyKey = '[$empty$]'

    angular.module('plunker', ['ui.bootstrap'])
      .directive('focusMe', function ($timeout, $parse) {
          return {
              //scope: true,   // optionally create a child scope
              link: function (scope, element, attrs) {
                  var model = $parse(attrs.focusMe);
                  scope.$watch(model, function (value) {
                      if (value === true) {
                          $timeout(function () {
                              element[0].focus();
                          });
                      }
                  });
              }
          };
      })

      .directive('emptyTypeahead', function () {
          return {
              require: 'ngModel',
              link: function (scope, element, attrs, modelCtrl) {
                  // this parser run before typeahead's parser
                  modelCtrl.$parsers.unshift(function (inputValue) {
                      var value = (inputValue ? inputValue : secretEmptyKey); // replace empty string with secretEmptyKey to bypass typeahead-min-length check
                      modelCtrl.$viewValue = value; // this $viewValue must match the inputValue pass to typehead directive
                      return value;
                  });

                  // this parser run after typeahead's parser
                  modelCtrl.$parsers.push(function (inputValue) {
                      return inputValue === secretEmptyKey ? '' : inputValue; // set the secretEmptyKey back to empty string
                  });
              }
          }
      })

        .controller('TypeaheadCtrl', function ($scope, $http, $timeout,limitToFilter) {
          $scope.selected = undefined;
          $scope.states = ['Alabama', 'Alaska', 'Arizona', 'Arkansas', 'California', 'Colorado', 'Connecticut', 'Delaware', 'Florida', 'Georgia', 'Hawaii', 'Idaho', 'Illinois', 'Indiana', 'Iowa', 'Kansas', 'Kentucky', 'Louisiana', 'Maine', 'Maryland', 'Massachusetts', 'Michigan', 'Minnesota', 'Mississippi', 'Missouri', 'Montana', 'Nebraska', 'Nevada', 'New Hampshire', 'New Jersey', 'New Mexico', 'New York', 'North Dakota', 'North Carolina', 'Ohio', 'Oklahoma', 'Oregon', 'Pennsylvania', 'Rhode Island', 'South Carolina', 'South Dakota', 'Tennessee', 'Texas', 'Utah', 'Vermont', 'Virginia', 'Washington', 'West Virginia', 'Wisconsin', 'Wyoming'];
          $scope.opened = false;

          $scope.stateComparator = function (state, viewValue) {
              return viewValue === secretEmptyKey || ('' + state).toLowerCase().indexOf(('' + viewValue).toLowerCase()) > -1;
          };

          $scope.onFocus = function (e) {
              $timeout(function () {
                  $(e.target).trigger('input');
                  $(e.target).trigger('change'); // for IE
              });
          };

          $scope.open = function () {
              $scope.opened = true;
          }
          $scope.close = function () {
              $scope.opened = false;
          }

          $scope.Search = function (Qury) {
              console.log("---------");
              console.log(Qury);
              $scope.SrchMessage = "Please Wait..";
              if (Qury != null && Qury != "[$empty$]") {
                  if (Qury.length >= 2) {
                      $http.jsonp('/Home/Search?Qry=' + Qury).then(function (data) {
                          if (data.data == "Fail") {
                              $scope.UpProfileMessage = "An Error has occured while loading posts!";
                          } else {
                              $scope.SrchMessage = "";
                              $scope.SearchResult = data.data;
                              $scope.ShowResult = true;
                              return limitToFilter(data.data, 15);
                          }
                      })
              .catch(function () {
                  $scope.error = "An Error has occured while loading posts!";
                  $scope.loading = false;
              });
                  }
              }
          }
      });
}());