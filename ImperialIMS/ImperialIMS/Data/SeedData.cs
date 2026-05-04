using ImperialIMS.Models;
using Microsoft.EntityFrameworkCore;

namespace ImperialIMS.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(ApplicationDbContext db)
        {
            if (await db.Categories.AnyAsync()) return;

            // --- Categories ---
            var weapons = new Category { Name = "Weapons", Description = "Imperial-grade blasters, rifles, and explosive ordnance" };
            var vehicles = new Category { Name = "Vehicles & Vessels", Description = "Starfighter components, walkers, and shuttlecraft parts" };
            var armor = new Category { Name = "Armor & Uniforms", Description = "Stormtrooper armor sets, officer uniforms, and specialized suits" };
            var comms = new Category { Name = "Communication Equipment", Description = "Comliks, holoprojectors, and encrypted transmission hardware" };
            var medical = new Category { Name = "Medical Supplies", Description = "Bacta, medkits, and field surgical equipment" };
            var power = new Category { Name = "Power Systems", Description = "Hypermatter cores, power cells, and shield generators" };
            var droids = new Category { Name = "Droids & Components", Description = "Droid units and spare component assemblies" };
            var detention = new Category { Name = "Detention Equipment", Description = "Restraints, interrogation hardware, and cell furnishings" };

            db.Categories.AddRange(weapons, vehicles, armor, comms, medical, power, droids, detention);
            await db.SaveChangesAsync();

            // --- Storage Facilities ---
            var deathStar = new StorageFacility { Name = "Death Star I — Sector 7G Armory", Location = "Horuz System, Scarif Orbit" };
            var coruscant = new StorageFacility { Name = "Coruscant Imperial Palace Armory", Location = "Coruscant, Imperial District" };
            var mustafar = new StorageFacility { Name = "Mustafar Imperial Facility", Location = "Mustafar, Northern Reaches" };
            var scarif = new StorageFacility { Name = "Scarif Citadel Tower Depot", Location = "Scarif, Tropical Archipelago" };
            var endor = new StorageFacility { Name = "Endor Shield Bunker Stockroom", Location = "Forest Moon of Endor" };
            var executor = new StorageFacility { Name = "Executor Supply Bay", Location = "Deep Space — Darth Vader's Fleet" };

            db.StorageFacilities.AddRange(deathStar, coruscant, mustafar, scarif, endor, executor);
            await db.SaveChangesAsync();

            // --- Items ---
            var e11 = new Item { Name = "E-11 Blaster Rifle", Description = "Standard-issue Imperial stormtrooper blaster; reliable, compact, lethal" };
            var dlt19 = new Item { Name = "DLT-19 Heavy Blaster Rifle", Description = "Long-range heavy blaster for suppression and anti-materiel roles" };
            var se14 = new Item { Name = "SE-14r Light Repeating Blaster", Description = "Compact sidearm favoured by Imperial officers and shore troopers" };
            var thermal = new Item { Name = "Thermal Detonator", Description = "Class-A explosive; requires authorization for issue" };
            var ionCannon = new Item { Name = "KDY v-150 Ion Cannon Emitter", Description = "Heavy planetary defence emplacement component; anti-capital-ship rated" };

            var tieWing = new Item { Name = "TIE Fighter Wing Panel", Description = "Hexagonal solar collector wing for TIE/ln starfighters" };
            var tieEngine = new Item { Name = "TIE Fighter Ion Engine", Description = "P-s4 twin ion engine assembly; primary propulsion unit" };
            var atatLeg = new Item { Name = "AT-AT Leg Assembly", Description = "Reinforced durasteel leg actuator for All Terrain Armored Transport" };
            var atatSt = new Item { Name = "AT-ST Cockpit Module", Description = "All Terrain Scout Transport upper hull and sensor dome" };
            var shuttleGear = new Item { Name = "Lambda-class Shuttle Landing Gear", Description = "Tri-foil retractable landing strut assembly" };

            var helmet = new Item { Name = "Stormtrooper Helmet", Description = "Phase III helmet with integrated comlink and HUD" };
            var chestPlate = new Item { Name = "Stormtrooper Chest Plate", Description = "Plastoid-composite torso armour, anterior section" };
            var officerUniform = new Item { Name = "Imperial Officer Uniform", Description = "Wool-blend duty uniform; rank plaque not included" };
            var snowtrooper = new Item { Name = "Snowtrooper Armour Set", Description = "Cold-weather assault trooper suit with heated undersuit" };
            var pilotSuit = new Item { Name = "TIE Pilot Flight Suit", Description = "Life-support-integrated sealed flight suit for TIE pilots" };

            var comlink = new Item { Name = "Comlink Unit", Description = "Short-range encrypted voice communicator" };
            var holoproj = new Item { Name = "Holographic Projector", Description = "Real-time three-dimensional communication projector" };
            var sensorArray = new Item { Name = "Long-Range Sensor Array", Description = "Passive/active phased-array sensor suite; 12-parsec range" };
            var encTx = new Item { Name = "Encrypted Transmission Module", Description = "HoloNet-compatible burst-encryption transponder" };

            var bactaTank = new Item { Name = "Imperial MedStat II Bacta Tank", Description = "Full-immersion regenerative bacta therapy unit" };
            var medkit = new Item { Name = "Field Medkit", Description = "Combat-grade medkit with coagulant, stim, and trauma dressing" };
            var stimpack = new Item { Name = "Stimpack Injector", Description = "Adrenaline-analogue combat stimulant auto-injector" };

            var reactorCore = new Item { Name = "Hypermatter Reactor Core", Description = "Miniaturised kyber-energy reactor; restricted clearance required" };
            var powerCell = new Item { Name = "Power Cell Array", Description = "Standardised Sienar-spec power cell bank (24-unit rack)" };
            var shieldGen = new Item { Name = "Deflector Shield Generator Module", Description = "Ray/particle deflector emitter for capital-ship installations" };

            var interoDroid = new Item { Name = "IT-O Interrogator Droid Chassis", Description = "Imperial Intelligence interrogation droid — partial assembly" };
            var mouseDroid = new Item { Name = "MSE-6 Mouse Droid Unit", Description = "Multi-purpose maintenance and courier droid" };
            var restraints = new Item { Name = "Stun Cuffs", Description = "Magnetically sealed wrist restraints with neural-shock capability" };

            db.Items.AddRange(
                e11, dlt19, se14, thermal, ionCannon,
                tieWing, tieEngine, atatLeg, atatSt, shuttleGear,
                helmet, chestPlate, officerUniform, snowtrooper, pilotSuit,
                comlink, holoproj, sensorArray, encTx,
                bactaTank, medkit, stimpack,
                reactorCore, powerCell, shieldGen,
                interoDroid, mouseDroid, restraints
            );
            await db.SaveChangesAsync();

            // --- Item Categories ---
            db.ItemCategories.AddRange(
                new ItemCategory { ItemId = e11.Id, CategoryId = weapons.Id },
                new ItemCategory { ItemId = dlt19.Id, CategoryId = weapons.Id },
                new ItemCategory { ItemId = se14.Id, CategoryId = weapons.Id },
                new ItemCategory { ItemId = thermal.Id, CategoryId = weapons.Id },
                new ItemCategory { ItemId = ionCannon.Id, CategoryId = weapons.Id },

                new ItemCategory { ItemId = tieWing.Id, CategoryId = vehicles.Id },
                new ItemCategory { ItemId = tieEngine.Id, CategoryId = vehicles.Id },
                new ItemCategory { ItemId = atatLeg.Id, CategoryId = vehicles.Id },
                new ItemCategory { ItemId = atatSt.Id, CategoryId = vehicles.Id },
                new ItemCategory { ItemId = shuttleGear.Id, CategoryId = vehicles.Id },

                new ItemCategory { ItemId = helmet.Id, CategoryId = armor.Id },
                new ItemCategory { ItemId = chestPlate.Id, CategoryId = armor.Id },
                new ItemCategory { ItemId = officerUniform.Id, CategoryId = armor.Id },
                new ItemCategory { ItemId = snowtrooper.Id, CategoryId = armor.Id },
                new ItemCategory { ItemId = pilotSuit.Id, CategoryId = armor.Id },

                new ItemCategory { ItemId = comlink.Id, CategoryId = comms.Id },
                new ItemCategory { ItemId = holoproj.Id, CategoryId = comms.Id },
                new ItemCategory { ItemId = sensorArray.Id, CategoryId = comms.Id },
                new ItemCategory { ItemId = encTx.Id, CategoryId = comms.Id },

                new ItemCategory { ItemId = bactaTank.Id, CategoryId = medical.Id },
                new ItemCategory { ItemId = medkit.Id, CategoryId = medical.Id },
                new ItemCategory { ItemId = stimpack.Id, CategoryId = medical.Id },

                new ItemCategory { ItemId = reactorCore.Id, CategoryId = power.Id },
                new ItemCategory { ItemId = powerCell.Id, CategoryId = power.Id },
                new ItemCategory { ItemId = shieldGen.Id, CategoryId = power.Id },

                new ItemCategory { ItemId = interoDroid.Id, CategoryId = droids.Id },
                new ItemCategory { ItemId = mouseDroid.Id, CategoryId = droids.Id },
                new ItemCategory { ItemId = interoDroid.Id, CategoryId = detention.Id },
                new ItemCategory { ItemId = restraints.Id, CategoryId = detention.Id }
            );
            await db.SaveChangesAsync();

            // --- Inventory Items ---

            // Death Star Armory
            var e11ds = new InventoryItem { ItemId = e11.Id, StorageFacilityId = deathStar.Id, StockCount = 12000, MaxStockLevel = 15000, ReorderLevel = 3000 };
            var dlt19ds = new InventoryItem { ItemId = dlt19.Id, StorageFacilityId = deathStar.Id, StockCount = 2400, MaxStockLevel = 3000, ReorderLevel = 600 };
            var thermalDs = new InventoryItem { ItemId = thermal.Id, StorageFacilityId = deathStar.Id, StockCount = 180, MaxStockLevel = 500, ReorderLevel = 100 };
            var helmetDs = new InventoryItem { ItemId = helmet.Id, StorageFacilityId = deathStar.Id, StockCount = 8500, MaxStockLevel = 10000, ReorderLevel = 2000 };
            var chestPlateDs = new InventoryItem { ItemId = chestPlate.Id, StorageFacilityId = deathStar.Id, StockCount = 8200, MaxStockLevel = 10000, ReorderLevel = 2000 };
            var reactorCoreDs = new InventoryItem { ItemId = reactorCore.Id, StorageFacilityId = deathStar.Id, StockCount = 4, MaxStockLevel = 10, ReorderLevel = 2 };
            var mouseDroidDs = new InventoryItem { ItemId = mouseDroid.Id, StorageFacilityId = deathStar.Id, StockCount = 340, MaxStockLevel = 500, ReorderLevel = 100 };
            var restraintsDs = new InventoryItem { ItemId = restraints.Id, StorageFacilityId = deathStar.Id, StockCount = 620, MaxStockLevel = 800, ReorderLevel = 150 };
            var interoDroidDs = new InventoryItem { ItemId = interoDroid.Id, StorageFacilityId = deathStar.Id, StockCount = 12, MaxStockLevel = 20, ReorderLevel = 5 };

            // Coruscant Imperial Palace Armory
            var officerUniformCs = new InventoryItem { ItemId = officerUniform.Id, StorageFacilityId = coruscant.Id, StockCount = 3200, MaxStockLevel = 4000, ReorderLevel = 800 };
            var se14Cs = new InventoryItem { ItemId = se14.Id, StorageFacilityId = coruscant.Id, StockCount = 5000, MaxStockLevel = 6000, ReorderLevel = 1200 };
            var holoprojCs = new InventoryItem { ItemId = holoproj.Id, StorageFacilityId = coruscant.Id, StockCount = 420, MaxStockLevel = 600, ReorderLevel = 100 };
            var encTxCs = new InventoryItem { ItemId = encTx.Id, StorageFacilityId = coruscant.Id, StockCount = 85, MaxStockLevel = 200, ReorderLevel = 50 };
            var comlinkCs = new InventoryItem { ItemId = comlink.Id, StorageFacilityId = coruscant.Id, StockCount = 9800, MaxStockLevel = 12000, ReorderLevel = 2500 };
            var bactaTankCs = new InventoryItem { ItemId = bactaTank.Id, StorageFacilityId = coruscant.Id, StockCount = 22, MaxStockLevel = 30, ReorderLevel = 8 };
            var medkitCs = new InventoryItem { ItemId = medkit.Id, StorageFacilityId = coruscant.Id, StockCount = 1400, MaxStockLevel = 2000, ReorderLevel = 400 };
                // Mustafar Imperial Facility
            var shieldGenMf = new InventoryItem { ItemId = shieldGen.Id, StorageFacilityId = mustafar.Id, StockCount = 7, MaxStockLevel = 20, ReorderLevel = 5 };
            var powerCellMf = new InventoryItem { ItemId = powerCell.Id, StorageFacilityId = mustafar.Id, StockCount = 320, MaxStockLevel = 500, ReorderLevel = 80 };
            var restraintsMf = new InventoryItem { ItemId = restraints.Id, StorageFacilityId = mustafar.Id, StockCount = 200, MaxStockLevel = 400, ReorderLevel = 80 };
            var interoDroidMf = new InventoryItem { ItemId = interoDroid.Id, StorageFacilityId = mustafar.Id, StockCount = 3, MaxStockLevel = 10, ReorderLevel = 3 };

                // Scarif Citadel Tower
            var e11Sc = new InventoryItem { ItemId = e11.Id, StorageFacilityId = scarif.Id, StockCount = 6200, MaxStockLevel = 8000, ReorderLevel = 1500 };
            var ionCannonSc = new InventoryItem { ItemId = ionCannon.Id, StorageFacilityId = scarif.Id, StockCount = 2, MaxStockLevel = 6, ReorderLevel = 2 };
            var sensorArraySc = new InventoryItem { ItemId = sensorArray.Id, StorageFacilityId = scarif.Id, StockCount = 18, MaxStockLevel = 30, ReorderLevel = 6 };
            var tieWingSc = new InventoryItem { ItemId = tieWing.Id, StorageFacilityId = scarif.Id, StockCount = 90, MaxStockLevel = 150, ReorderLevel = 30 };
            var tieEngineSc = new InventoryItem { ItemId = tieEngine.Id, StorageFacilityId = scarif.Id, StockCount = 55, MaxStockLevel = 150, ReorderLevel = 30 };
                // Endor Shield Bunker
            var snowtrooperEndor = new InventoryItem { ItemId = snowtrooper.Id, StorageFacilityId = endor.Id, StockCount = 400, MaxStockLevel = 600, ReorderLevel = 120 };
            var e11Endor = new InventoryItem { ItemId = e11.Id, StorageFacilityId = endor.Id, StockCount = 1800, MaxStockLevel = 3000, ReorderLevel = 600 };
            var comlinkEndor = new InventoryItem { ItemId = comlink.Id, StorageFacilityId = endor.Id, StockCount = 1100, MaxStockLevel = 2000, ReorderLevel = 400 };
            var medkitEndor = new InventoryItem { ItemId = medkit.Id, StorageFacilityId = endor.Id, StockCount = 210, MaxStockLevel = 500, ReorderLevel = 100 };
            var stimpackEndor = new InventoryItem { ItemId = stimpack.Id, StorageFacilityId = endor.Id, StockCount = 75, MaxStockLevel = 300, ReorderLevel = 80 };
                // Executor Supply Bay
            var pilotSuitExecutor = new InventoryItem { ItemId = pilotSuit.Id, StorageFacilityId = executor.Id, StockCount = 1200, MaxStockLevel = 1500, ReorderLevel = 300 };
            var tieWingExecutor = new InventoryItem { ItemId = tieWing.Id, StorageFacilityId = executor.Id, StockCount = 240, MaxStockLevel = 400, ReorderLevel = 80 };
            var tieEngineExecutor = new InventoryItem { ItemId = tieEngine.Id, StorageFacilityId = executor.Id, StockCount = 190, MaxStockLevel = 400, ReorderLevel = 80 };
            var atatLegExecutor = new InventoryItem { ItemId = atatLeg.Id, StorageFacilityId = executor.Id, StockCount = 16, MaxStockLevel = 40, ReorderLevel = 10 };
            var atatStExecutor = new InventoryItem { ItemId = atatSt.Id, StorageFacilityId = executor.Id, StockCount = 6, MaxStockLevel = 20, ReorderLevel = 5 };
            var shuttleGearExecutor = new InventoryItem { ItemId = shuttleGear.Id, StorageFacilityId = executor.Id, StockCount = 30, MaxStockLevel = 60, ReorderLevel = 12 };
            var shieldGenExecutor = new InventoryItem { ItemId = shieldGen.Id, StorageFacilityId = executor.Id, StockCount = 3, MaxStockLevel = 15, ReorderLevel = 4 };
            db.InventoryItems.AddRange(e11ds, dlt19ds, thermalDs, helmetDs, chestPlateDs, reactorCoreDs, mouseDroidDs, restraintsDs, interoDroidDs);
            db.InventoryItems.AddRange(officerUniformCs, se14Cs, holoprojCs, encTxCs, comlinkCs, bactaTankCs, medkitCs);
            db.InventoryItems.AddRange(shieldGenMf, powerCellMf, restraintsMf, interoDroidMf);
            db.InventoryItems.AddRange(e11Sc, ionCannonSc, sensorArraySc, tieWingSc, tieEngineSc);
            db.InventoryItems.AddRange(snowtrooperEndor, e11Endor, comlinkEndor, medkitEndor, stimpackEndor);
            db.InventoryItems.AddRange(pilotSuitExecutor, tieWingExecutor, tieEngineExecutor, atatLegExecutor, atatStExecutor, shuttleGearExecutor, shieldGenExecutor);
            await db.SaveChangesAsync();

            // --- Shipments ---
            var shipment1 = new Shipment
            {
                RequestDate = new DateTime(2026, 3, 1),
                EstimatedDeliveryDate = new DateTime(2026, 3, 10),
                ReceivedDate = new DateTime(2026, 3, 10),
                TrackingId = 77001,
                Status = ShippingStatus.Delivered,
                ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8",
                DeliveryLocation = deathStar
            };
            var shipment2 = new Shipment
            {
                RequestDate = new DateTime(2026, 3, 15),
                EstimatedDeliveryDate = new DateTime(2026, 3, 25),
                ReceivedDate = default,
                TrackingId = 77002,
                Status = ShippingStatus.InTransit,
                ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8",
                DeliveryLocation = executor
            };
            var shipment3 = new Shipment
            {
                RequestDate = new DateTime(2026, 4, 1),
                EstimatedDeliveryDate = new DateTime(2026, 4, 12),
                ReceivedDate = default,
                TrackingId = 77003,
                Status = ShippingStatus.Delayed,
                ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8",
                DeliveryLocation = endor
            };
            var shipment4 = new Shipment
            {
                RequestDate = new DateTime(2026, 4, 10),
                EstimatedDeliveryDate = new DateTime(2026, 4, 20),
                ReceivedDate = default,
                TrackingId = 77004,
                Status = ShippingStatus.Pending,
                ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8",
                DeliveryLocation = scarif
            };
            var shipment5 = new Shipment
            {
                RequestDate = new DateTime(2026, 2, 14),
                EstimatedDeliveryDate = new DateTime(2026, 2, 20),
                ReceivedDate = default,
                TrackingId = 77005,
                Status = ShippingStatus.Lost,
                ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8",
                DeliveryLocation = mustafar
            };

            db.Shipments.AddRange(shipment1, shipment2, shipment3, shipment4, shipment5);
            await db.SaveChangesAsync();

            // --- Manifests ---
            db.Manifests.AddRange(
                // Shipment 1 — delivered to Death Star
                new Manifest { InventoryItemId = e11ds.Id, ShippingId = shipment1.Id, amount = 500 },
                new Manifest { InventoryItemId = helmetDs.Id, ShippingId = shipment1.Id, amount = 300 },
                new Manifest { InventoryItemId = chestPlateDs.Id, ShippingId = shipment1.Id, amount = 300 },
                new Manifest { InventoryItemId = mouseDroidDs.Id, ShippingId = shipment1.Id, amount = 40 },

                // Shipment 2 — in transit to Executor
                new Manifest { InventoryItemId = tieWingSc.Id, ShippingId = shipment2.Id, amount = 60 },
                new Manifest { InventoryItemId = tieEngineSc.Id, ShippingId = shipment2.Id, amount = 60 },
                new Manifest { InventoryItemId = pilotSuitExecutor.Id, ShippingId = shipment2.Id, amount = 100 },

                // Shipment 3 — delayed to Endor
                new Manifest { InventoryItemId = snowtrooperEndor.Id, ShippingId = shipment3.Id, amount = 120 },
                new Manifest { InventoryItemId = stimpackEndor.Id, ShippingId = shipment3.Id, amount = 200 },
                new Manifest { InventoryItemId = medkitEndor.Id, ShippingId = shipment3.Id, amount = 150 },

                // Shipment 4 — pending to Scarif
                new Manifest { InventoryItemId = sensorArraySc.Id, ShippingId = shipment4.Id, amount = 8 },
                new Manifest { InventoryItemId = ionCannonSc.Id, ShippingId = shipment4.Id, amount = 2 },
                new Manifest { InventoryItemId = e11Sc.Id, ShippingId = shipment4.Id, amount = 800 },

                // Shipment 5 — lost (Mustafar)
                new Manifest { InventoryItemId = reactorCoreDs.Id, ShippingId = shipment5.Id, amount = 3 },
                new Manifest { InventoryItemId = shieldGenMf.Id, ShippingId = shipment5.Id, amount = 5 }
            );
            await db.SaveChangesAsync();

            // Retrieve inventory items needed for alerts
            var lowTieEngine = await db.InventoryItems.FirstAsync(i => i.ItemId == tieEngine.Id && i.StorageFacilityId == scarif.Id);
            var lowShieldMustafar = await db.InventoryItems.FirstAsync(i => i.ItemId == shieldGen.Id && i.StorageFacilityId == mustafar.Id);
            var lowStimEndor = await db.InventoryItems.FirstAsync(i => i.ItemId == stimpack.Id && i.StorageFacilityId == endor.Id);
            var lowReactor = await db.InventoryItems.FirstAsync(i => i.ItemId == reactorCore.Id && i.StorageFacilityId == deathStar.Id);

            // --- Alerts ---
            db.Alerts.AddRange(
                new Alert
                {
                    Message = "TIE Fighter Ion Engine stock at Scarif is critically low (55 units). Reorder threshold is 30.",
                    alertType = AlertType.LowStock,
                    InventoryItemId = lowTieEngine.Id,
                    ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8"
                },
                new Alert
                {
                    Message = "Deflector Shield Generator stock at Mustafar Facility is below reorder level (7 of 5 threshold).",
                    alertType = AlertType.LowStock,
                    InventoryItemId = lowShieldMustafar.Id,
                    ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8"
                },
                new Alert
                {
                    Message = "Stimpack Injector stock at Endor Shield Bunker is critically low (75 units, threshold 80). Combat readiness at risk.",
                    alertType = AlertType.LowStock,
                    InventoryItemId = lowStimEndor.Id,
                    ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8"
                },
                new Alert
                {
                    Message = "Hypermatter Reactor Core supply at Death Star Armory approaching minimum (4 units). Lord Vader has been informed.",
                    alertType = AlertType.LowStock,
                    InventoryItemId = lowReactor.Id,
                    ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8"
                },
                new Alert
                {
                    Message = "Shipment #77003 to Endor Shield Bunker is delayed. Snowtrooper reinforcement timeline compromised.",
                    alertType = AlertType.Delay,
                    ShipmentId = shipment3.Id,
                    ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8"
                },
                new Alert
                {
                    Message = "Shipment #77005 to Mustafar has been marked LOST. Hypermatter cores unaccounted for. Imperial Security Bureau notified.",
                    alertType = AlertType.Delay,
                    ShipmentId = shipment5.Id,
                    ApplicationUserId = "807240fd-ef7e-4ece-9ae5-649985e1c3a8"
                }
            );
            await db.SaveChangesAsync();
        }
    }
}
