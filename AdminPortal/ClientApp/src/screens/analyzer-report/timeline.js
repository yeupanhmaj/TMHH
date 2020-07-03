import React, { Component } from 'react';
import moment from 'moment';
import { Container } from 'reactstrap';
import { VerticalTimeline, VerticalTimelineElement } from 'react-vertical-timeline-component';
import 'react-vertical-timeline-component/style.min.css';
import 'react-vertical-timeline-component/docs/main.css';
import { FcCheckmark } from "react-icons/fc";
import * as Service from '../../services/reportService';

export default class Timeline extends Component {
    constructor(props) {

        super(props);
        this.state = {
            data: []
        };
    }
    componentDidMount() {
        Service.getProposalProccess().then((response) => {
            if (response.isSuccess == true) {
                for (let item of response.data) {
                    for (let proccess of item.lstProccess)
                        proccess.date = moment(new Date(proccess.date)).format('DD-MM-YYYY');
                }
                this.setState({ data: response.data })
             
            }
        })
    };
    render() {
        let { data } = this.state;
        const listItems = data.map((item, index) =>
            <VerticalTimeline>
                <h1 /*style={{ textAlign: 'center' }}*/>{item.proposalCode}</h1>
                {item.lstProccess.map((proccess) =>
                    <VerticalTimelineElement
                        //className="vertical-timeline-element--work"
                        contentStyle={{ background: 'white' }}
                        contentArrowStyle={{ borderRight: '7px solid  rgb(33, 150, 243)' }}
                        iconStyle={{ backgroundColor: 'PaleGreen' }}
                        icon={<FcCheckmark />}
                        date={proccess.date}>

                        <h3 className="vertical-timeline-element-title">{proccess.id}</h3>
                        <h4 className="vertical-timeline-element-subtitle">{proccess.code}</h4>

                    </VerticalTimelineElement>
                )}
            </VerticalTimeline>
        );
        return (
            <Container style={{ paddingTop: "100px", backgroundColor: 'AliceBlue' }}>
                <ul>{listItems}</ul>
            </Container>);
    }
}
