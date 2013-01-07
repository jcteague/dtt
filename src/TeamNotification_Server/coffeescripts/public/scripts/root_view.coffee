define 'root_view', ['general_view','config','navbar_view','breadcrumb_view','form_view'], (GeneralView, config, NavbarView, BreadcrumbView, FormView) ->

    class RootView extends GeneralView

        id: 'root-container'
        registerview: $("""<div class='span4'></div>""")
        left_form: $("""<div class='span4'></div>""")
        middle: $("""<div class='span4'></div>""")
        row: $("""<div class='row'></div>""")
        initialize: ->
            @navbar_view = new NavbarView(model:@model)
            @breadcrumb = new BreadcrumbView(model:@model)
        render: ->
            @$el.empty()
            @$el.attr('class', 'row')
            @registerview.empty()
            @left_form.empty()
            @middle.empty()
            @row.empty()
            if @model.has('root') 
                links = @model.get('root').links
                reg = @model.get('registration')
                login = @model.get('login')
                @navbar_view.render().append_to @$el
                @model.attributes = reg
                @formview = new FormView(model:@model)
                @formview.on 'all', @propagate_event, @
                
                @$el.append """<div class="row hero-unit">
  <h1>Welcome to Yackety</h1>
  <p>Some awesome placeholder tagline ;D</p>
  <p>
    <a class="btn btn-primary btn-large">
      Learn more
    </a>
  </p>
</div>"""
                @$el.append @row
                @row.append @left_form
                @row.append @middle
                @row.append @registerview
                @registerview.append "<h3>Sing up</h3>"
                @formview.render().append_to @registerview
                @middle.append """<h3>Contact us</h3>
                    <p>We just met you and this is crazy but here's or email so... contact us, maybe?<br/> You can emai us your inquiries to info[at]yakett.com <br/>You can call our talfree service at some phone number 1-200-YAK-ETTY</p>
                """
                @left_form.append """<h3>Features</h3>
                <ul>
                    <li>Boost your team communication</li>
                    <li>Create rooms and organize your teams</li>
                </ul>
                 
                """
            @
        anchor_click_handler:(obj)->
            $('.active').attr('class','');
            
            
        append_to: (parent) ->
            @$el.appendTo parent
