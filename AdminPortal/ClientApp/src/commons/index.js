

import { ProposalDefine } from '../models/proposal';
import { ExplanationDefine } from '../models/explanation';
import { SurveyDefine } from '../models/survey';
import { QuoteDefine } from '../models/quote';
import { BidPlanDefine } from '../models/bidPlan';
import { AuditDefine } from '../models/audit';
import { NegotiationDefine } from '../models/negotiation';
import { DecisionDefine } from '../models/decision';
import { DeliveryReceiptDefine } from '../models/deliveryReceipt';
import { AcceptanceDefine } from '../models/acceptance';
import { ContractDefine } from '../models/contract';
import { EmployeeDefine } from '../models/employee';
import { CapitalDefine } from '../models/capital';

export const CONST_FEATURE = {
    Proposal: {
        feature: 'Proposal',
        KEY_COLUMN: 'proposalID',
        itemDef: ProposalDefine,
    },
    Explanation: {
        feature: 'Explanation',
        KEY_COLUMN: 'explanationID',
        itemDef: ExplanationDefine,

    },
    Survey: {
        feature: 'Survey',
        KEY_COLUMN: 'surveyID',
        itemDef: SurveyDefine,
    },
    Quote: {
        feature: 'Quote',
        KEY_COLUMN: 'quoteID',
        itemDef: QuoteDefine,
    },
    Audit: {
        feature: 'Audit',
        KEY_COLUMN: 'auditID',
        itemDef: AuditDefine,
    },
    BidPlan: {
        feature: 'BidPlan',
        KEY_COLUMN: 'bidPlanID',
        itemDef: BidPlanDefine,
    },
    Negotiation: {
        feature: 'Negotiation',
        KEY_COLUMN: 'negotiationID',
        itemDef: NegotiationDefine,
    },
    Decision: {
        feature: 'Decision',
        KEY_COLUMN: 'decisionID',
        itemDef: DecisionDefine,
    },
    Contract: {
        feature: 'Contract',
        KEY_COLUMN: 'contractID',
        itemDef: ContractDefine,
    },
    DeliveryReceipt: {
        feature: 'DeliveryReceipt',
        KEY_COLUMN: 'deliveryReceiptID',
        itemDef: DeliveryReceiptDefine,
    },
    Acceptance: {
        feature: 'Acceptance',
        KEY_COLUMN: 'acceptanceID',
        itemDef: AcceptanceDefine,
    },
    Employee: {
        feature: 'Employee',
        KEY_COLUMN: 'employeeID',
        NAME_COLUMN: 'name',
        itemDef: EmployeeDefine,
    },
    Capital: {
        feature: 'Capital',
        KEY_COLUMN: 'capitalID',
        itemDef: CapitalDefine,
    }
}

