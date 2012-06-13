define 'client_view', ['backbone', 'client_router', 'form_template_renderer'], (Backbone, ClientRouter, FormTemplateRenderer) ->

    class ClientView extends Backbone.View

        initialize: ->
            @setElement '#client-content'
            @router = new ClientRouter()
            @router.on 'render', @render_path, @
            Backbone.history.start()

        render: ->
            @$el.empty()
            @render_links() if @links?
            @render_template() if @template?
            @

        render_path: (path) ->
            $.getJSON(path, @load_json)

        load_json: (data) =>
            console.log 'got this:', data
            @links = data.links
            @template = data.template
            @render()

        render_links: ->
            @$el.append('<div id="links"><h1>Links</h1></div>')
            @append_link link for link in @links

        append_link: (link) ->
            @$('#links').append """
                <p>
                    <a href="##{link.href}">#{link.rel}</a>
                </p>
            """

        render_template: ->
            @$el.append('<div id="form-container"><h1>Form</h1></div>')
            return
