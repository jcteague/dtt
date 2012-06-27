define 'query_view', ['backbone', 'query_renderer'], (Backbone, QueryRenderer) ->

    class QueryView extends Backbone.View

        id: 'queries-container'

        events:
            'submit': 'submit_form'

        initialize: ->
            @query_renderer = new QueryRenderer()

        render: ->
            @$el.empty()
            if @model?
                @$el.append('<h1>Queries</h1>')
                @$el.append(@query_renderer.render(@model))
            @delegateEvents(@events)
            @

        update: (queries) ->
            @model = queries

        append_to: (parent) ->
            @$el.appendTo parent

        submit_form: (event) ->
            event.preventDefault()
            data = {}
            @$('input').not(':submit').each () ->
                $current = $(this)
                data[$current.attr('name')] = $current.val()

            $.post(@$('form').attr('action'), data, (res) -> null)
