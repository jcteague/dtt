define 'user_edit_view', ['general_view', 'form_view', 'links_view'], (GeneralView, FormView, LinksView) ->

    class UserEditView extends GeneralView

        id: 'user-edit-container'

        initialize: ->
            @form_view = new FormView(model: @model)
            @links_view = new LinksView(model: @model)

            @form_view.on 'response:received', @check_user_edit

        check_user_edit: (res) =>
            if res.success is true
                window.location.href = "/#/user/#{res.data.id}"
            else
                @trigger 'messages:display', res.server_messages

        render: ->
            @$el.empty()
            @links_view.render().append_to @$el
            @form_view.render().append_to @$el
            @
