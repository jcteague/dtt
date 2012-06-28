define 'client_view', ['backbone', 'client_router', 'form_view', 'links_view', 'query_view', 'messages_view'], (Backbone, ClientRouter, FormView, LinksView, QueryView, MessagesView) ->

    class ClientView extends Backbone.View

        initialize: ->
            @setElement '#client-content'
            @router = new ClientRouter()
            @links_view = new LinksView()
            @form_view = new FormView()
            @query_view = new QueryView()
            @messages_view = new MessagesView()
<<<<<<< HEAD

            @subscribe_to_events()
=======
>>>>>>> Added the messages view
            Backbone.history.start()

        render: ->
            @$el.empty()
            @links_view.render().append_to @$el
            @form_view.render().append_to @$el
            @query_view.render().append_to @$el
            @messages_view.render().append_to @$el
            @

        subscribe_to_events: ->
            @router.on 'render', @render_path, @
            @form_view.on 'messages:display', @display_messages, @
            @query_view.on 'messages:display', @display_messages, @

        render_path: (path) ->
            $.getJSON(path, @load_json)

        load_json: (data) =>
            @data = data
            @links_view.update data.links
            @form_view.update data
            @query_view.update data.queries
            @messages_view.update data.messages
            @render()

        display_messages: (messages) ->
            @messages_view.update(messages)
            @messages_view.render().append_to @$el
