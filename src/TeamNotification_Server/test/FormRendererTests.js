/**
 * Created by JetBrains WebStorm.
 * User: jcteague
 * Date: 6/12/12
 * Time: 9:57 PM
 * To change this template use File | Settings | File Templates.
 */
var mocha = require("mocha")
    $ = require('jquery')
    ,should = require("should")
    ,sinon = require("sinon")


var FormTemplateRenderer = function(collection){
    var textFieldBuilder = function(template){
        return [
            $('<label>',{"for": template.name}),
            $('<input>',{"type":"text","name":template.name})
        ];
    };
    var templatedFieldBuilder = function(template){
        //load a template
    }
    var generatorSelector = function(fieldType){
        if(fieldType === 'string') return textFieldBuilder;
    }

    var form = $('<form>',{action:collection.uri});
    var form_templates = collection.template.data;
    console.log(form_templates);
    form_templates.forEach(function(template){
        var fieldGenerator = generatorSelector(template.type);
        var field_elements = fieldGenerator(template);
        console.log(field_elements);
        field_elements.forEach(function(f){form.append(f)});

    })
    return form;

}

var collection = {
    uri: "collectionuri",
    data:[],
    template:{
        data:[
            {name:"fieldA",type:"string",label:"label"}
        ]
    }
};

describe("When Renering the Form from the collection template",function(){
    it("return a form html tag",function(){

        var form = FormTemplateRenderer(collection);
        form.attr('action').should.equal(collection.uri);

    })
    it("should render a label with the label field",function(){
        var form = FormTemplateRenderer(collection);

        var childElements = form.children();

        label[0].tagName.should.equal("LABEL");

    })
})