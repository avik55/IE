'use strict';

var app = angular.module('IndEntnApp', ['lr.upload']);
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

//app.controller('PhtUpladJsController', ['$scope', 'upload', '$http', function ($scope, upload, $http) {
//    //Update Cover Image
//    $scope.ShowCovetImageModel = true;
//    $scope.coverImgMessage = "----------------";

//    $scope.doUpload = function () {

//        upload({
//            url: '../Home/upload',
//            method: 'POST',
//            data: {
//                aFile: $scope.myFile
//            }
//        }).then(
//          function (response) {
//              console.log(response.data);
//          },
//          function (response) {
//              console.error(response);
//          }
//        );
//    }
//}]);

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

