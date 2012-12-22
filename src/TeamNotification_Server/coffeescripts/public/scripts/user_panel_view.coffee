define 'user_panel_view', ['general_view', 'config','navbar_view','breadcrumb_view'], (GeneralView, config, NavbarView,BreadcrumbView) ->

    class UserPanelView extends GeneralView

        id: 'user-panel-container'
        initialize: ->
            @navbar = new NavbarView(model:@model)
            @breadcrumb = new BreadcrumbView(model:@model)
            console.log @model
        render: ->
            @$el.empty()
            @navbar.render().append_to @$el
            @breadcrumb.render().append_to @$el
            @$el.append '<h1>Your rooms</h1>'
            
            @$el.append
            @
