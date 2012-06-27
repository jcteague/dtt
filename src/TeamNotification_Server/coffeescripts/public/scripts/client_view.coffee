define 'client_view', ['backbone', 'client_router', 'form_view', 'links_view', 'query_view'], (Backbone, ClientRouter, FormView, LinksView, QueryView) ->

    class ClientView extends Backbone.View

        initialize: ->
            @setElement '#client-content'
            @router = new ClientRouter()
            @router.on 'render', @render_path, @
            @links_view = new LinksView()
            @form_view = new FormView()
            @query_view = new QueryView()
            Backbone.history.start()

        render: ->
            @$el.empty()
            @links_view.render().append_to @$el
            @form_view.render().append_to @$el
            @query_view.render().append_to @$el
            @

        render_path: (path) ->
            $.getJSON(path, @load_json)

        load_json: (data) =>
            @data = data
            @links_view.update data.links
            @form_view.update data
            @query_view.update data.queries
            @render()
