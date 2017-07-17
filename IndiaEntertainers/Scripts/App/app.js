'use strict';
var secretEmptyKey = '[$empty$]'

var app = angular.module('IndEntnApp', ['lr.upload', 'ngMessages', 'ngAnimate', 'ngSanitize', 'ui.bootstrap']);
//app.config.$inject = ['$routeProvider', '$locationProvider'];

//Not Necessary to Create Service, Same can be done in COntroller also as method like add() method
app.service('filteredListService', function () {
    this.searched = function (valLists, toSearch) {
        return _.filter(valLists,
        function (i) {
            /* Search Text in all 3 fields */
            return searchUtil(i, toSearch);
        });
    };
});

app.directive('modal', function () {
    return {
        template: '<div class="modal fade">' +
            '<div class="modal-dialog" style="width:900px;">' +
              '<div class="modal-content">' +
                '<div class="modal-header">' +
                  '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
                  '<h4 class="modal-title">{{ title }}</h4>' +
                '</div>' +
                '<div class="modal-body" ng-transclude></div>' +
              '</div>' +
            '</div>' +
          '</div>',
        restrict: 'E',
        transclude: true,
        replace: true,
        scope: true,
        link: function postLink(scope, element, attrs) {
            scope.title = attrs.title;
            scope.$watch(attrs.visible, function (value) {
                if (value == true)
                    $(element).modal('show');
                else
                    $(element).modal('hide');
            });
            $(element).on('shown.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = true;
                });
            });
            $(element).on('hidden.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = false;
                });
            });
        }
    };
});

//For User Validation.
angular.module('UserValidation', []).directive('validPasswordC', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue, $scope) {
                var noMatch = viewValue != scope.myForm.password.$viewValue
                ctrl.$setValidity('noMatch', !noMatch)
            })
        }
    }
})

app.directive('uploadFile', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var file_uploaded = $parse(attrs.uploadFile);
            element.bind('change', function () {
                scope.$apply(function () {
                    file_uploaded.assign(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

//////////////////////
//app.directive('focusMe', function ($timeout, $parse) {
//    return {
//        //scope: true,   // optionally create a child scope
//        link: function (scope, element, attrs) {
//            var model = $parse(attrs.focusMe);
//            scope.$watch(model, function (value) {
//                if (value === true) {
//                    $timeout(function () {
//                        element[0].focus();
//                    });
//                }
//            });
//        }
//    };
//})

//app.directive('emptyTypeahead', function () {
//    return {
//        require: 'ngModel',
//        link: function (scope, element, attrs, modelCtrl) {
//            // this parser run before typeahead's parser
//            modelCtrl.$parsers.unshift(function (inputValue) {
//                var value = (inputValue ? inputValue : secretEmptyKey); // replace empty string with secretEmptyKey to bypass typeahead-min-length check
//                modelCtrl.$viewValue = value; // this $viewValue must match the inputValue pass to typehead directive
//                return value;
//            });

//            // this parser run after typeahead's parser
//            modelCtrl.$parsers.push(function (inputValue) {
//                return inputValue === secretEmptyKey ? '' : inputValue; // set the secretEmptyKey back to empty string
//            });
//        }
//    }
//})

