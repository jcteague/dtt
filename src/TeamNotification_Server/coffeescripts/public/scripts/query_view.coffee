define 'query_view', ['backbone', 'query_renderer'], (Backbone, QueryRenderer) ->

    class QueryView extends Backbone.View

        id: 'queries-container'

        initialize: ->
            @query_renderer = new QueryRenderer()

        render: ->
            @$el.empty()
            if @model?
                @$el.append('<h1>Queries</h1>')
                @$el.append(@query_renderer.render(collection))
            @

        update: (queries) ->
            @model = queries

        append_to: (parent) ->
            @$el.appendTo parent
