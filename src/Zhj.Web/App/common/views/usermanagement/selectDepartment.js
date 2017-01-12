(function () {
    appModule.controller('common.views.usermanagement.selectDepartment', [
        '$scope', '$uibModalInstance', 'abp.services.app.user','abp.services.app.order',
        function ($scope, $uibModalInstance, userService,orderService) {
            var vm = this;

            ///机构树
            vm.organizationTree = {
                $tree: null,
                unitCount: 0,
                setUnitCount: function (unitCount) {
                    $scope.safeApply(function () {
                        vm.organizationTree.unitCount = unitCount;
                    });
                },
                refreshUnitCount: function () {
                    vm.organizationTree.setUnitCount(vm.organizationTree.$tree.jstree('get_json').length);
                },
                selectedOu: {
                    id: null,
                    displayName: null,
                    code: null,
                    set: function (ouInTree) {
                        if (!ouInTree) {
                            vm.organizationTree.selectedOu.id = null;
                            vm.organizationTree.selectedOu.displayName = null;
                            vm.organizationTree.selectedOu.code = null;
                            vm.organizationTree.selectedOu.type = 1;
                        } else {
                            vm.organizationTree.selectedOu.id = ouInTree.id;
                            vm.organizationTree.selectedOu.displayName = ouInTree.original.displayName;
                            vm.organizationTree.selectedOu.code = ouInTree.original.code;
                            vm.organizationTree.selectedOu.type = ouInTree.original.type;
                        }
                        // vm.getorgusers(1);
                    }
                },

                contextMenu: function (node) {
                    var items = {};
                    return items;
                },


                generateTextOnTree: function (ou) {
                    var displayName = ou.name;
                    displayName = displayName.length > 35 ? (displayName.substring(0, 35) + "...") : displayName;
                    var itemClass = ' ou-text-no-members';
                    return '<span title="' + ou.name + '" class="ou-text' + itemClass + '  section-h' +
                        '" data-ou-id="' + ou.id + '">' +
                        '<i class="fa fa-university"></i>' +
                        '<span class="organi-displayName">' + displayName + '</span></span>';
                },

                incrementMemberCount: function (ouId, incrementAmount) {
                    var treeNode = vm.organizationTree.$tree.jstree('get_node', ouId);
                    treeNode.original.memberCount = treeNode.original.memberCount + incrementAmount;
                    vm.organizationTree.$tree.jstree('rename_node',
                        treeNode,
                        vm.organizationTree.generateTextOnTree(treeNode.original));
                },

                getTreeDataFromServer: function (callback) {
                    orderService.getDepartmentList({
                      
                    }).success(function (result) {
                        result.items = result.result;

                        var searchlist = [];
                        if (vm.orgname) {
                            angular.forEach(result.items, function (aa) {
                                aa.parent_id = null;
                                if (aa.name.indexOf(vm.orgname) >= 0) {
                                    searchlist.push(aa);
                                }

                            });
                        }
                        if (vm.orgname)
                            result.items = searchlist;
                        vm.orglist = result.items;

                        var treeData = _.map(result.items, function (item) {
                            return {
                                id: item.id, type: item.type,
                                parent: item.parent_id ? item.parent_id : '#',
                                displayName: item.name,
                                text: vm.organizationTree.generateTextOnTree(item),
                                state: {
                                    opened: true
                                }
                            };
                        });
                        callback(treeData);
                    });
                },

                init: function () {
                    vm.organizationTree.getTreeDataFromServer(function (treeData) {
                        vm.organizationTree.setUnitCount(treeData.length);
                        vm.organizationTree.$tree = $('#OrganizationUnitEditTree');
                        var jsTreePlugins = [
                            'types',
                            'contextmenu',
                            'wholerow',
                            'sort'
                        ];

                        vm.organizationTree.$tree
                            .on('changed.jstree', function (e, data) {
                                $scope.safeApply(function () {
                                    if (data.selected.length != 1) {
                                        vm.organizationTree.selectedOu.set(null);
                                    } else {
                                        var selectedNode = data.instance.get_node(data.selected[0]);
                                        vm.organizationTree.selectedOu.set(selectedNode);
                                    }
                                });

                            })
                            .on('move_node.jstree', function (e, data) {


                                var parentNodeName = (!data.parent || data.parent == '#')
                                    ? app.localize('Root')
                                    : vm.organizationTree.$tree.jstree('get_node', data.parent).original.displayName;


                            })
                            .jstree({
                                'core': {
                                    data: treeData,
                                    multiple: false,
                                    check_callback: function (operation, node, node_parent, node_position, more) {
                                        return true;
                                    }
                                },
                                types: {
                                    "default": {
                                        "icon": "fa"
                                    },
                                    "file": {
                                        "icon": "fa"
                                    }
                                },
                                contextmenu: {
                                    items: vm.organizationTree.contextMenu
                                },
                                sort: function (node1, node2) {
                                    if (this.get_node(node2).original.displayName < this.get_node(node1).original.displayName) {
                                        return 1;
                                    }

                                    return -1;
                                },
                                plugins: jsTreePlugins
                            });

                        vm.organizationTree.$tree.on('click', '.ou-text .fa-caret-down', function (e) {
                            e.preventDefault();

                            var ouId = $(this).closest('.ou-text').attr('data-ou-id');
                            setTimeout(function () {
                                vm.organizationTree.$tree.jstree('show_contextmenu', ouId);
                            }, 100);
                        });
                    });
                },

                reload: function () {
                    vm.organizationTree.getTreeDataFromServer(function (treeData) {
                        vm.organizationTree.setUnitCount(treeData.length);
                        vm.organizationTree.$tree.jstree(true).settings.core.data = treeData;
                        vm.organizationTree.$tree.jstree('refresh');
                    });
                }
            };
            vm.search = function () {
                vm.organizationTree.reload();
            };
            vm.saveorg = function () {
                var ac = vm.organizationTree.selectedOu;
                var temp = { id: ac.id, name: ac.displayName }
                $uibModalInstance.close(temp);
            };

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
            vm.organizationTree.init();
        }


    ]);
})();