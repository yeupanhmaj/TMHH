
import moment from 'moment';
import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as ProposalService from '../../../services/proposalService';
import * as Acceptance from './acceptance';
import * as Audit from './audit';
import * as BidPlan from './bidPlan';
import * as Contract from './contract';
import * as Decision from './decision';
import * as DeliveryReceipt from './deliveryReceipt';
import * as Explanation from './explanation';
import * as Negotiation from './negotiation';
import * as Quote from './quote';
import * as Survey from './survey';
import * as Proposal from './proposal';


export const CONST_COMMON = {
    Proposal: {
        func: Proposal.ProposalModify,
        itemDef: CONST_FEATURE.Proposal.itemDef,
        init: Proposal.init 
    },
    Explanation: {
        func: Explanation.ExplanationModify,
        itemDef: CONST_FEATURE.Explanation.itemDef,
        init: Explanation.init 
    },
    Survey: {
        func: Survey.SurveyModify,
        itemDef: CONST_FEATURE.Survey.itemDef,
        init: Survey.init 
    },
    Quote: {
        func: Quote.QuoteModify,
        itemDef: CONST_FEATURE.Quote.itemDef,
        init: Quote.init 
    },
    Audit: {
        func: Audit.AuditModify,
        itemDef: CONST_FEATURE.Audit.itemDef,
        init: Audit.init 
    },
    BidPlan: {
        func: BidPlan.BidPlanModify,
        itemDef: CONST_FEATURE.BidPlan.itemDef,
        init: BidPlan.init 
    },
    Negotiation: {
        func: Negotiation.NegotiationModify,
        itemDef: CONST_FEATURE.Negotiation.itemDef,
        init: Negotiation.init 
    },
    Decision: {
        func: Decision.DecisionModify,
        itemDef: CONST_FEATURE.Decision.itemDef,
        init: Decision.init 
    },
    Contract: {
        func: Contract.ContractModify,
        itemDef: CONST_FEATURE.Contract.itemDef,
        init: Contract.init 
    },
    DeliveryReceipt: {
        func: DeliveryReceipt.DeliveryReceiptModify,
        itemDef: CONST_FEATURE.DeliveryReceipt.itemDef,
        init: DeliveryReceipt.init 
    },
    Acceptance: {
        func: Acceptance.AcceptanceModify,
        itemDef: CONST_FEATURE.Acceptance.itemDef,
        init: Acceptance.init 

    }
}




