import vi from "date-fns/locale/vi";
import * as React from 'react';
import { registerLocale } from "react-datepicker";
import { Route, Switch } from 'react-router';
import './commons/css/main.css';
import Authenticated from './components/authenticated';
import Layout from './components/layout';
import Acceptance from './screens/acceptance';
import Audit from './screens/audit';
import BidPlan from './screens/bidPlan';
import Capital from './screens/capital';
import Contract from './screens/contract';
import Decision from './screens/decision';
import DeliveryReceipt from './screens/deliveryReceipt';
import Employee from './screens/employee';
import Explanation from './screens/explanation';
import Login from './screens/login';
import Negotiation from './screens/negotiation';
import Proposal from './screens/newScreens/proposal';
import Quote from './screens/quote';
import Survey from './screens/survey';
import UnknowPage from './screens/unknowPage';
import Users from './screens/users';
import Analyzer from './screens/analyzer';
import Practice from './screens/practice';
import Report from './screens/report';
import ReportAnalyzer from './screens/analyzer-report';
import ConfirmationForm from './screens/confirmform';
import ReturnForm from './screens/returnform';
import Branch from "./screens/branch";
import './custom.css';
import 'antd/dist/antd.css';

registerLocale("vi", vi);
export default () => (
    <Layout >
        <Switch>
            <Route exact path='/login' component={Login} />
            <Authenticated exact path='/' component={Report} />
            <Authenticated exact path='/de-xuat' component={Proposal} />
            <Authenticated exact path='/khaosat' component={Survey} />
            <Authenticated exact path='/giaitrinh' component={Explanation} />
            <Authenticated exact path='/bao-gia-kem-de-xuat' component={Quote} />
            <Authenticated exact path='/bien-ban-hop-gia' component={Audit} />
            <Authenticated exact path='/lap-ke-hoach-chon-thau' component={BidPlan} />
            <Authenticated exact path='/bien-ban-thuong-thao-hd' component={Negotiation} />
            <Authenticated exact path='/ho-so-quyet-dinh-chon-thau' component={Decision} />
            <Authenticated exact path='/lap-ho-hop-dong' component={Contract} />
            <Authenticated exact path='/bien-ban-giao-nhan' component={DeliveryReceipt} />
            <Authenticated exact path='/bien-ban-nghiem-thu' component={Acceptance} />
            <Authenticated exact path='/quan-ly-nhan-vien' component={Employee} />
            <Authenticated exact path='/nguon-von' component={Capital} />
            <Authenticated exact path='/quan-ly-nguoi-dung' component={Users} />
            <Authenticated exact path='/Practice' component={Practice} />
            <Authenticated exact path='/report' component={Report} />
            <Authenticated exact path='/reportAnalyzer' component={ReportAnalyzer} />
            <Authenticated exact path='/quan-ly-tai-san' component={Analyzer} />
            <Authenticated exact path='/quan-ly-chi-nhanh' component={Branch} />
            <Authenticated exact path='/confirmform' component={ConfirmationForm} />
            <Authenticated exact path='/form-hoan-tra' component={ReturnForm} />
            {/* <Authenticated exact  path='/inbieumau' component={PrintTemplate} />  */}


            <Authenticated component={UnknowPage} />
        </Switch>
    </Layout>
);
