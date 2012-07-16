define 'client_view', ['backbone', 'client_router', 'form_view', 'links_view', 'query_view', 'server_response_view', 'views_factory'], (Backbone, ClientRouter, FormView, LinksView, QueryView, ServerResponseView, ViewsFactory) ->

    class ClientView extends Backbone.View

        views: []

        initialize: ->
            @setElement '#client-content'
            @router = new ClientRouter()
            @links_view = new LinksView()
            @form_view = new FormView()
            @query_view = new QueryView()
            @server_response_view = new ServerResponseView()
            @views_factory = new ViewsFactory()

            @subscribe_to_events()
            Backbone.history.start()

        render: ->
            @$el.empty()
            @links_view.render().append_to @$el
            @form_view.render().append_to @$el
            @query_view.render().append_to @$el
            @server_response_view.render().append_to @$el
            view.render().append_to(@$el) for view in @views
            @

        subscribe_to_events: ->
            @router.on 'render', @render_path, @
            @form_view.on 'messages:display', @display_messages, @
            @query_view.on 'messages:display', @display_messages, @
            @form_view.on 'all', @propagate_event, @

        propagate_event: (event, values) ->
            @trigger event, values
            @render()

        render_path: (path) ->
            $.getJSON(path, @load_json)

        load_json: (data) =>
            @data = data
            @links_view.update @data.links
            @form_view.update @data
            @query_view.update @data.queries

            @views = @views_factory.get_for @data
            view.listen_to @ for view in @views
            @render()

        display_messages: (messages) ->
            @server_response_view.update(messages)
            @server_response_view.render().append_to @$el
