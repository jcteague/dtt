define 'query_view', ['general_view', 'query_renderer'], (GeneralView, QueryRenderer) ->

    class QueryView extends GeneralView

        id: 'queries-container'

        events:
            'submit': 'submit_form'

        initialize: ->
            @model.on 'change:queries', @render, @
            @query_renderer = new QueryRenderer()

        render: ->
            @$el.empty()
            @$el.attr('class','hero-unit')
            if @model.has('queries')
                @$el.append('<h2>Queries</h2>')
                @$el.append(@query_renderer.render(@model.get('queries')))
            @delegateEvents(@events)
            @

        append_to: (parent) ->
            @$el.appendTo parent

        submit_form: (event) ->
            event.preventDefault()

            func = =>
                data = {}
                @$('input').not(':submit').each () ->
                    $current = $(this)
                    data[$current.attr('name')] = $current.val()

                $.post @$('form').attr('action'), data, (res) => 
                    @$('input').not(':submit').val('')
                    @trigger 'messages:display', res.messages

            setTimeout func, 200
