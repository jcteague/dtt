define 'client_view', ['backbone', 'client_router', 'form_view', 'links_view', 'query_view', 'server_response_view', 'views_factory', 'collection_model'], (Backbone, ClientRouter, FormView, LinksView, QueryView, ServerResponseView, ViewsFactory, CollectionModel) ->

    class ClientView extends Backbone.View

        views: []

        initialize: ->
            @model = new CollectionModel
            @setElement '#client-content'
            @router = new ClientRouter()
            @server_response_view = new ServerResponseView(model: @model)
            @router.on 'render', @render_path, @
            @views_factory = new ViewsFactory()
            Backbone.history.start()

        render: ->
            @$el.empty()
            view.render().append_to(@$el) for view in @views
            @server_response_view.render().append_to @$el
            prettyPrint()
            @

        subscribe_to_events: ->
            for view in @views
                if view instanceof FormView
                    view.on 'messages:display', @display_messages, @
                    view.on 'all', @propagate_event, @
                if view instanceof QueryView 
                    view.on 'messages:display', @display_messages, @

        propagate_event: (event, values) ->
            @trigger event, values

        render_path: (path) ->
            @server_response_view.clear()
            $.getJSON(path, @load_json)

        load_json: (data) =>
            @model.clear()
            @model.set data
            @views = @views_factory.get_for @model
            @subscribe_to_events()
            view.listen_to @ for view in @views
            @render()

        display_messages: (messages) ->
            @server_response_view.update(messages)
            @server_response_view.render().append_to @$el
