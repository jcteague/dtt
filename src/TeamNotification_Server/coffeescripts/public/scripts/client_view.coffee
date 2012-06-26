define 'client_view', ['backbone', 'client_router', 'form_template_renderer', 'query_renderer'], (Backbone, ClientRouter, FormTemplateRenderer, QueryRenderer) ->

    class ClientView extends Backbone.View

        events:
            'submit': 'submit_form'

        initialize: ->
            @setElement '#client-content'
            @router = new ClientRouter()
            @router.on 'render', @render_path, @
            @form_template_renderer = new FormTemplateRenderer()
            @query_renderer = new QueryRenderer()
            Backbone.history.start()

        render: ->
            @$el.empty()
            if @data?
                @render_links() if @data.links?
                @render_template() if @data.template?
                @render_queries() if @data.queries?
            @

        render_path: (path) ->
            $.getJSON(path, @load_json)

        load_json: (data) =>
            @data = data
            @render()

        render_links: ->
            @$el.append('<div id="links"><h1>Links</h1></div>')
            @append_link link for link in @data.links

        append_link: (link) ->
            @$('#links').append """
                <p>
                    <a href="##{link.href}">#{link.name}</a>
                </p>
            """

        render_template: ->
            @$el.append('<div id="form-container"><h1>Form</h1></div>')
            console.log 'rendered template', @form_template_renderer.render(@data)
            @$el.append(@form_template_renderer.render(@data))

        render_queries: ->
            @$el.append('<div id="form-container"><h1>Queries</h1></div>')
            console.log 'rendered queries', @query_renderer.render(@data)
            @$el.append(@query_renderer.render(@data))

        submit_form: (event) ->
            event.preventDefault()
            url = _.find(@data.links, (item) ->
                item.name is 'self'
            )
            data = {}
            $('input').not(':submit').each () ->
                $current = $(this)
                data[$current.attr('name')] = $current.val()

            $.post(url.href, data, (res) -> console.log(res))
