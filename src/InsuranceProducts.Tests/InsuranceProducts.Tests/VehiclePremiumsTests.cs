using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests
{
    public class VehiclePremiumsTests
    {
        [Fact]
        public void VehicleTypes()
        {
            var listVehicleType = new List<VehicleType>
            {
                new VehicleType{ Code = "1", Description = "ประเภทรถยนต์นั่ง" },
                new VehicleType{ Code = "2", Description = "ประเภทรถยนต์โดยสาร" },
                new VehicleType{ Code = "3", Description = "ประเภทรถยนต์บรรทุก" },
                new VehicleType{ Code = "4", Description = "ประเภทรถยนต์ลากจูง" },
                new VehicleType{ Code = "5", Description = "ประเภทรถพ่วง" },
                new VehicleType{ Code = "6", Description = "ประเภทรถจักรยานยนต์" },
                new VehicleType{ Code = "7", Description = "ประเภทรถยนต์นั่งรับจ้างสาธารณะ" },
                new VehicleType{ Code = "8", Description = "ประเภทรถยนต์เบ็ดเตล็ด" },
            };

            var listVehicleUsage = new List<VehicleUsage>
            {
                new VehicleUsage{ Code = "10", Description = "ชนิดรถยนต์ส่วนบุคคล" },
                new VehicleUsage{ Code = "20", Description = "ชนิดรถยนต์ใช้เพื่อการพาณิชย์" },
                new VehicleUsage{ Code = "30", Description = "ชนิดรถยนต์ใช้รับจ้างสาธารณะ" },
                new VehicleUsage{ Code = "40", Description = "ชนิดรถยนต์ใช้เพื่อการพาณิชย์พิเศษ" },

                // สำหรับประเภทรถยนต์บรรทุกที่ใช้ลากจูง
                new VehicleUsage{ Code = "27", Description = "ชนิดรถยนต์บรรทุกใช้ลากจูงรถพ่วงเพื่อการพาณิชย์" },
                new VehicleUsage{ Code = "47", Description = "ชนิดรถยนต์บรรทุกใช้ลากจูงรถพ่วงเพื่อการพาณิชย์พิเศษ" },

                // สำหรับประเภทรถยนต์เบ็ดเตล็ด
                new VehicleUsage{ Code = "01", Description = "รถยนต์ป้ายแดง" },
                new VehicleUsage{ Code = "02", Description = "รถพยาบาล" },
                new VehicleUsage{ Code = "03", Description = "รถดับเพลิง" },
                new VehicleUsage{ Code = "04", Description = "รถใช้ในการเกษตร" },
                new VehicleUsage{ Code = "05", Description = "รถใช้ในการก่อสร้าง" },
                new VehicleUsage{ Code = "06", Description = "รถอื่นๆ" },
            };


            // ขนาดรถยนต์
            // 110, 120 = CC [0, 2000], [2001, INF]
            // 210, 220, 230 = Seat [0, 20], [21, 40], [41, INF]
            // 320, 340 = kg [0, 4000], [4001, 12000], [12001, INF]
            // 327, 347 = kg [0, 4000]
            // 420 = kg [0, 8000], [8001, INF]
            // 520 = kg [0, 30000], [30001, INF]
            // 540 = kg [0, 30000], [30001, INF]
            // 610, 620, 630 = CC [0, 125], [126, 250], [251, INF]
            // 730 = CC [0, 1000], [1001, 2000], [2001, INF]
            // *801
            // 802 = CC [0, 2000], [2001, INF]
            // 803, 804, 805 = kg [0, 12000], [12001, INF]
            // *806


        }

        [Fact]
        public void VehiclePremiumsStep()
        {
            // ตารางที่ 1 เบี้ยประกันภัยพื้นฐาน
            // ตารางที่ 2 อัตราเบี้ยประกันภัยเพิ่มตามความเสี่ยงภัย
            // - (1) ลักษณะการใช้รถยนต์
            // - (2) ขนาดรถยนต์
            // - (3) พฤติกรรมการขับขี่ของผู้ขับขี่
            // - (4) กลุ่มรถยนต์
            // ขั้นที่ 1 -- g1 [5000001, INF]
            //       -- g2 [1500001, 5000000]
            //       -- g3 [1000001, 1500000]
            //       -- g4 [700001, 1000000]
            //       -- g5 [0, 700000]
            // ขั้นที่ 2 ให้ปรับกลุ่มรถยนต์ที่ได้ในขั้นที่ 1 ขึ้นอีก 1 ขั้น กรณีลักษณะของรถเป็นรถยนต์สปอร์ต หรือ เป็นรถที่นำเข้าจากต่างประเทศ
            // ตารางที่ 3 อัตราเบี้ยประกันภัยเพิ่มความคุ้มครอง
            // ตารางที่ 4 อัตราเบี้ยประกันภัยสำหรับการประกันภัยเพิ่มตามเอกสารแนบท้าย
            var g1 = new Dictionary<string, List<string>>
            {
                { "ASTON", ["MARTIN"] },
                { "BENTLEY", [] },
                { "CADILLAC", [] },
                { "DAIMLER", [] },
                { "FERRARI", [] },
                { "HONDA", ["LEGEND", "NSX", "ODYSSEY", "PRELUDE"] },
                { "JAGUAR", [] },
                { "MASERATI", [] },
                { "MG", [] },
                { "NISSAN", ["PRESIDENT", "INFINITTY"] },
                { "PORSCHE", [] },
                { "ROLLS-ROYCE", [] },
                { "TOYOTA", ["SUPRA"] },
            };
            var g2 = new Dictionary<string, List<string>>
            {
                { "ALFA", ["ROMEO"] },
                { "AUDI", [] },
                { "BMW", [] },
                { "CHRYSLER", [] },
                { "CITROEN", [] },
                { "HOLDEN", [] },
                { "ISUZU", ["TROOPER"] },
                { "JEEP-CHEROKEE", [] },
                { "LAND-ROVER", [] },
                { "LEXUS", [] },
                { "MAGSO", [] },
                { "MAZDA", ["MX5"] },
                { "MERCEDES-BENZ", [] },
                { "MITSUBISHI", ["PAJERO"] },
                { "OPEL", ["CALIBRA", "OMAGA", "VECTRA"] },
                { "PEUGEOT", [] },
                { "ROVER", [] },
                { "SAAB", [] },
                { "TOYOTA", ["CELICA", "CROWN", "LAND", "CRUISER"] },
                { "VOLKSWAGEN", ["RAV-4"] },
                { "VOLVO", [] },
            };
            var g3 = new Dictionary<string, List<string>>
            {
                { "CHEVROLET", ["ZAFIRA"] },
                { "DAIHATSU", ["GRAN", "TERIOS"] },
                { "FIAT", [] },
                { "FORD", [] },
                { "HONDA", ["ACCORD", "CIVIC", "CRV"] },
                { "HYUNDAI", [] },
                { "ISUZU", ["CAMEO", "RODEO", "VEGA", "VERTEX"] },
                { "KIA", [] },
                { "MAZDA", ["121", "626", "ASTINA", "CRONOS", "LANTIS"] },
                { "MITSUBISHI", ["GALANT"] },
                { "NISSAN", ["CEFIRO", "PRIMERA", "200SX", "121"] },
                { "OPEL", ["ASTRA", "CORSA"] },
                { "PROTON", ["SEGA"] },
                { "SEAT", [] },
                { "SUBARU", ["IMPREZA", "LEGACY"] },
                { "SUZUKI", ["ESTEEM", "VITARA"] },
                { "TOYOTA", ["CAMRY", "CORONA", "STARLET"] },
            };
            var g4 = new Dictionary<string, List<string>>
            {
                { "MAZDA", ["323"] },
                { "MITSUBISHI", ["CHAMP"] },
                { "TOYOTA", ["COROLLA", "SPORT-RIDER"] },
                { "CHEVROLET", ["OPTRA"] },
            };
            var g5 = new Dictionary<string, List<string>>
            {
                { "DAIHATSU", ["MIRA"] },
                { "HONDA", ["CITY"] },
                { "MAZDA", ["FAMILIA"] },
                { "NISSAN", ["NV", "SUNNY"] },
                { "SUZUKI", ["CARRIBIAN"] },
                { "TOYOTA", ["SOLUNA"] }
            };

            // (5) อายุรถยนต์ *อายุรถยนต์ใช้สำหรับรถทุกรหัส ยกเว้นรถรหัส 801 (รถยนต์ป้ายแดง)
            // (6) จำนวนเงินเอาประกันภัย 
            // (7) อุปกรณ์เพิ่มพิเศษ *อุปกรณ์เพิ่มพิเศษใช้สำหรับรถรหัส 320 รหัส 327 รหัส 340 รหัส 347 รหัส 420 รหัส 520 และรหัส 540

            // ตารางที่ 3 อัตราเบี้ยประกันภัยเพิ่มความคุ้มครอง
            // (1) ความรับผิดต่อความบาดเจ็บหรือมรณะของบุคคลภายนอก และความรับผิดต่อความบาดเจ็บหรือ มรณะของผู้โดยสารในรถยนต์คันเอาประกันภัย(บจ.)
            // (2) ความรับผิดต่อทรัพย์สินของบุคคลภายนอก (ทส.)

            // ตารางที่ 4 อัตราเบี้ยประกันภัยสำหรับการประกันภัยเพิ่มตามเอกสารแนบท้าย
            // (1) การประกันภัยอุบัติเหตุส่วนบุคคล
            // (2) การประกันภัยค่ารักษา ยาบาล
            // (3) การประกันตัวผู้ขับขี่
            // (4) คุ้มครองภัยน้ำท่วม
            // (5) คุ้มครองภัยธรรมชาติ
            // (6) การซ่อมศูนย์บริการ
            // -- ประเภทรถยนต์นั่ง (1*) ไม่เกิน 2.3% ของจำนวนเงินเอาประกันภัย [0, 2.3%]
            // -- ประเภทรถยนต์โดยสาร (2*) ไม่เกิน 2.1% ของจำนวนเงินเอาประกันภัย [0, 2.1%]
            // -- ประเภทรถยนต์บรรทุก (3*) ไม่เกิน 4.3% ของจำนวนเงินเอาประกันภัย [0, 4.3%]

            // 10. ส่วนลดและส่วนเพิ่มเบี้ยประกันภัยการประกันภัยรถยนต์ภาคสมัครใจ
            // 10.1 การประกันภัยกลุ่ม
            // -- ตั้งแต่ 3 คัน ขึ้นไป จะได้รับส่วนลดจำนวน 10 % ของเบี้ยประกันภัยในรถยนต์แต่ละคัน
            // -- การที่ผู้เอาประกันภัย คู่สมรส บิดา มารดา บุตร หรือพี่น้องร่วมบิดา หรือมารดาเดียวกัน
            // ของผู้เอาประกันภัย มีรถยนต์เอาประกันภัยไว้กับบริษัทรวมกันตั้งแต่ 3 คันขึ้นไป บริษัทอาจให้ส่วนลดได้ไม่
            // เกิน 10 %
            // 10.2 อัตราเบี้ยประกันภัยประวัติดี และอัตราเบี้ยประกันภัยประวัติไม่ดีสำหรับลักษณะการใช้ส่วนบุคคล
            // 10.3.1 การเพิ่มเบี้ยประกันภัยประวัติดี
            // ขั้นที่ 1 20% ปีแรก
            // ขั้นที่ 2 30% ปีสองติด
            // ขั้นที่ 3 40% ปีสามติด
            // 10.3.2 การเพิ่มเบี้ยประกันภัยประวัติไม่ดี *2 ครั้งขึ้นไปรวมกันมีจำนวนเงินเกิน 200% ของเบี้ยประกันภัย
            // ขั้นที่ 1 20% ของอัตราเบี้ยประกันภัยในปีที่ต่ออายุ
            // ขั้นที่ 2 30% ของอัตราเบี้ยประกันภัยในปีที่ต่ออายุ ในกรณีมีค่าเสียหายดังกล่าวเกิดขึ้นต่อบริษัท 2 ปีติดต่อกัน
            // ขั้นที่ 3 40% ของอัตราเบี้ยประกันภัยในปีที่ต่ออายุ ในกรณีมีค่าเสียหายดังกล่าวเกิดขึ้นต่อบริษัท 3 ปีติดต่อกัน
            // ขั้นที่ 4 50% ของอัตราเบี้ยประกันภัยในปีที่ต่ออายุ ในกรณีมีค่าเสียหายดังกล่าวเกิดขึ้นต่อบริษัท 4 ปีติดต่อกัน หรือกว่านั้น

            // 10.4 ความเสียหายส่วนแรก
            // 10.5 ส่วนลดอื่น
            // - CCTV อัตราร้อยละ 5–10 ของประกันภัยสุทธิ
            // - รติดตั้งแอปพลิเคชันสำหรับการแจ้งอุบัติเหตุรถยนต์ อัตราไม่เกินร้อยละ 10 ของเบี้ยประกันภัยสุทธิ

            // ตารางที่ 1 เบี้ยประกันภัยื้นฐาน
            var poType1 = "type_1";
            var poType2 = "type_2";
            var poType3 = "type_3";
            var basePremims = new List<BasePremiums>
            {
                new BasePremiums(poType1, 7600, 12000),
                new BasePremiums(poType2, 3000, 5000),
                new BasePremiums(poType3, 2200, 3000),
            };

            // ตารางที่ 2 อัตราเบี้ยประกันภัยเิ่มตามความเสี่ยงภัย
            // ลักษณะการใช้รถยนต์
            // - การใช้ส่วนบุคคล
            var personal = "PERSONAL";
            var comercial = "COMERCIAL";
            var usageRiskPremiums = new List<RiskPremiums>
            {
                // ประเภท 1
                new RiskPremiums(poType1, personal, 1),
                new RiskPremiums(poType1, comercial, 1.05m),

                // ประเภท 2
                new RiskPremiums(poType2, personal, 1),
                new RiskPremiums(poType2, comercial, 1.05m),

                // ประเภท 3
                new RiskPremiums(poType3, personal, 1),
                new RiskPremiums(poType3, comercial, 1.05m),
            };
            // - การใช้เพื่อการพาณิชย์

        }
    }

    public record VehicleType
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
    }

    public record VehicleUsage
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
    }

    public record BasePremiums(string PolicyType, decimal MinPremiums, decimal MaxPremiums);
    public record RiskPremiums(string PolicyType, string Code, decimal Rate);
}
