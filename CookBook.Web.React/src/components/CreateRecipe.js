import React, { Component } from 'react';
import { Button, Form } from 'react-bulma-components';
import { createRecipe, uploadFiles } from '../models/recipes';

const { Input, Field, Control, Label, Textarea, InputFile } = Form;

export class CreateRecipe extends Component {

    constructor(props) {
        super(props);
        this.state = {
            title: '',
            description: '',
            instructions: '',
            ingredients: '',
            servings: 1,
            recipeAlbumDocumentID: null
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }

    onFileChange = async event => {
        const recipeAlbumDocumentID = await uploadFiles(event.target.files);
        this.setState({
            ...this.state,
            recipeAlbumDocumentID: recipeAlbumDocumentID
        });
    }

    async handleSubmit(event) {
        event.preventDefault();
        await createRecipe(this.state);
        this.props.history.push("/");
    }

    render() {
        return (
            <div>
                <h1>Create Recipe</h1>
                <form onSubmit={this.handleSubmit}>
                    <Field>
                        <Control>
                            <Label>Title</Label>
                            <Input
                                type="text"
                                name="title"
                                value={this.state.title}
                                onChange={this.handleChange} />
                        </Control>
                    </Field>
                    <Field>
                        <Control>
                            <Label>Number of servings</Label>
                            <Input
                                type="number"
                                min="1"
                                name="servings"
                                value={this.state.servings}
                                onChange={this.handleChange} />
                        </Control>
                    </Field>
                    <Field>
                        <Label>Pictures</Label>
                        <InputFile name="pictures" boxed inputProps={{ multiple: true }} onChange={this.onFileChange}>
                        </InputFile>
                        </Field>
                    <Field>
                        <Label>Description</Label>
                        <Control>
                            <Textarea
                                name="description"
                                value={this.state.description}
                                onChange={this.handleChange} />
                        </Control>
                    </Field>
                    <Field>
                        <Label>Instructions</Label>
                        <Control>
                            <Textarea
                                name="instructions"
                                value={this.state.instructions}
                                onChange={this.handleChange} />
                        </Control>
                    </Field>
                    <Field>
                        <Label>Ingredients</Label>
                        <Control>
                            <Textarea
                                name="ingredients"
                                value={this.state.ingredients}
                                onChange={this.handleChange} />
                        </Control>
                    </Field>
                    <Button className="is-primary" onClick={this.handleSubmit}>Create</Button>
                </form>
            </div>
        );
    }
}