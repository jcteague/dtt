define 'user_edit_view', ['general_view', 'form_view', 'links_view', 'navbar_view', 'breadcrumb_view'], (GeneralView, FormView, LinksView, NavbarView, BreadcrumbView) ->

    class UserEditView extends GeneralView

        id: 'user-edit-container'

        initialize: ->
            @form_view = new FormView(model: @model)
            breadcrumb_links = [
                { name:'Home', href:"/user", rel:"BreadcrumbLink" }
                { name:'Edit Profile', href:"/user/#{@model.user_id}/edit", rel:"active" }
            ]
            @model.set('breadcrumb', breadcrumb_links)
            @breadcrumb = new BreadcrumbView(model:@model)
            @navbar_view = new NavbarView(model: @model)
            @form_view.on 'response:received', @check_user_edit

        check_user_edit: (res) =>
            if res.success is true
                window.location.href = "/#/user/#{res.data.id}"
            else
                @trigger 'messages:display', res.server_messages

        render: ->
            @$el.empty()
            @navbar_view.render().append_to @$el
            @breadcrumb.render().append_to @$el
            
            #@links_view.render().append_to @$el
            @form_view.render().append_to @$el
            @
