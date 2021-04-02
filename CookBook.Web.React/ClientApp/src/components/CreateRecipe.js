import React, { Component } from 'react';
import { Button, Form } from 'react-bulma-components';
import { createRecipe } from '../models/recipes';

const { Input, Field, Control, Label, Textarea } = Form;

export class CreateRecipe extends Component {

    constructor(props) {
        super(props);
        this.state = {
            title: '',
            description: '',
            instructions: '',
            ingredients: '',
            servings: 1
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

    async handleSubmit(event) {
        event.preventDefault();
        await createRecipe(this.state);
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