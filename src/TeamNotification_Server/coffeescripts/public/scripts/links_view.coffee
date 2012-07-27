define 'links_view', ['general_view'], (GeneralView) ->

    class LinksView extends GeneralView

        id: 'links'
        initialize: ->
            @model.on 'change:links', @render, @

        render: ->
            @$el.empty()
            @$el.attr('class', 'hero-unit')
            if @model.has('links')
                @$el.append('<h2>Links</h2>')
                @append_link link for link in @model.get('links')
            @
        append_to: (parent) ->
            @$el.appendTo parent

        append_link: (link) ->
            @$el.append """
                <p>
                    <a href="##{link.href}">#{link.name}</a>
                </p>
            """
