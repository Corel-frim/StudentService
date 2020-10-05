import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { groups: [], loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderGroupsTable(groups) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Count of students</th>
          </tr>
        </thead>
        <tbody>
        {groups.map(group =>
            <tr key={group.id}>
                <td>{group.id}</td>
                <td>{group.name}</td>
                <td>{group.studentsCount}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderGroupsTable(this.state.groups);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('group/list');
    const data = await response.json();
      this.setState({ groups: data, loading: false });
  }
}
