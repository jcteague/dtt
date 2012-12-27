define 'client_view', ['backbone', 'client_router', 'form_view', 'links_view', 'query_view', 'user_edit_view', 'login_view', 'messages_view', 'server_response_view', 'views_factory', 'collection_model', 'jquery.ie.cors', 'config'], (Backbone, ClientRouter, FormView, LinksView, QueryView, UserEditView, LoginView, MessagesView, ServerResponseView, ViewsFactory, CollectionModel, IECors, config) ->

    class ClientView extends Backbone.View

        views: []

        initialize: ->
            @model = new CollectionModel
            @setElement '#client-content'
            @router = new ClientRouter()
            @server_response_view = new ServerResponseView()
            @router.on 'render', @render_path, @
            @views_factory = new ViewsFactory()
            Backbone.history.start()

        display_messages: (messages) ->
            @server_response_view.update(messages)
            @server_response_view.render().append_to @$el
            
        render: ->
            @$el.empty()
            view.render().append_to(@$el) for view in @views
            if( typeof(@model.attributes.server_messages) != 'undefined')
                @display_messages @model.attributes.server_messages 
            else
                @server_response_view.render().append_to @$el
            if $('.prettyprint')?
                prettyPrint()
                $('.prettyprint').removeClass('prettyprint')
            @

        subscribe_to_events: ->
            for view in @views
                if view instanceof FormView
                    view.on 'all', @propagate_event, @
                if @can_display_messages view
                    view.on 'messages:display', @display_messages, @

        can_display_messages: (view) ->
            #console.log view
            (view instanceof QueryView) or (view instanceof UserEditView) or (view instanceof FormView) or (view instanceof LoginView)
            
        propagate_event: (event, values) ->
            @trigger event, values

        render_path: (path) ->
            @server_response_view.clear()
            path = '' if path is '/'
            url = "#{config.api.url}/#{path}"
            parameters = {
                type: 'GET'
                dataType: 'json'
                url: url
                cache: false
                headers:
                    Authorization: $.cookie 'authtoken'
                success: @load_json
                error: (d) -> return
            }

            $.ajax parameters

        load_json: (data) =>
            @model.clear()
            @model.set data
            @views = @views_factory.get_for @model
            @subscribe_to_events()
            view.listen_to @ for view in @views
            @render()

