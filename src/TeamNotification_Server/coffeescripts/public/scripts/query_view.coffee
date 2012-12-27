define 'query_view', ['general_view', 'query_renderer', 'config'], (GeneralView, QueryRenderer, config) ->

    class QueryView extends GeneralView

        id: 'queries-container'

        events:
            'submit': 'submit_form'

        initialize: ->
            @model.on 'change:queries', @render, @
            @query_renderer = new QueryRenderer()

        render: ->
            @$el.empty()
            #@$el.attr('class','hero-unit')
            if @model.has('queries')
               # @$el.append('<h2>Queries</h2>')
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

                callback = (res) => 
                    @$el.find('input').not(':submit').val('')
                    console.log 'here'
                    #@$('input').not(':submit').val('')
                    @trigger 'messages:display', res.server_messages

                url = "#{config.api.url}#{@$('form').attr('action')}"
                parameters = {
                    type: 'POST'
                    data: data
                    url: url
                    success: callback
                    error: (d) -> return
                }

                if $.cookie('authtoken')?
                    parameters.beforeSend = (jqXHR) ->
                        authToken = $.cookie 'authtoken'
                        jqXHR.setRequestHeader('Authorization', authToken )
                        jqXHR.withCredentials = true

                $.ajax parameters

            setTimeout func, 200
