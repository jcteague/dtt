define 'client_view', ['backbone', 'client_router', 'form_view', 'links_view', 'query_view', 'server_response_view', 'messages_view'], (Backbone, ClientRouter, FormView, LinksView, QueryView, ServerResponseView, MessagesView) ->

    class ClientView extends Backbone.View

        initialize: ->
            @setElement '#client-content'
            @router = new ClientRouter()
            @links_view = new LinksView()
            @form_view = new FormView()
            @query_view = new QueryView()
            @messages_view = new MessagesView()
            @server_response_view = new ServerResponseView()
            @subscribe_to_events()
            Backbone.history.start()

        render: ->
            #@$el.empty()
            @$el.contents().remove()
            @links_view.render().append_to @$el
            @form_view.render().append_to @$el
            @query_view.render().append_to @$el
            @messages_view.render().append_to @$el if typeof @messages_view != 'undefined'
            @

       # update_messages: (unformatted_new_message) ->
       #     @messages_view.add_message @data.messages, unformatted_new_message
       #     @messages_view.update @data.messages
       #     @messages_view.render().append_to @$el
            
        subscribe_to_events: ->
            @router.on 'render', @render_path, @
            @query_view.on 'messages:display', @display_messages, @
            @form_view.on 'messages:display', @display_messages, @
            #@form_view.on 'response:received',@update_messages, @

        render_path: (path) ->
            $.getJSON(path, @load_json)

        load_json: (data) =>
            @data = data
            @links_view.update @data.links
            @form_view.update @data
            @query_view.update @data.queries
            @messages_view.update @data
            @render()

        display_messages: (messages) ->
            @server_response_view.update(messages)
            @server_response_view.render().append_to @$el
